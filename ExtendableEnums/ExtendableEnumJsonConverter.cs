using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using Newtonsoft.Json;

namespace ExtendableEnums
{
    /// <summary>
    /// Converts ExtendableEnum objects to and from JSON.
    /// </summary>
    public class ExtendableEnumJsonConverter : JsonConverter
    {
        private static readonly ConcurrentDictionary<Type, MethodInfo> parseValueMethodCache = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, MethodInfo> parseMethodCache = new ConcurrentDictionary<Type, MethodInfo>();

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (objectType == null)
            {
                throw new ArgumentNullException(nameof(objectType));
            }

            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            var valueType = GetTypeOfValueParameter(objectType);
            var parseValueMethod = GetParseValueMethod(objectType);

            var rawValue = reader.Value;
            if (rawValue == null)
            {
                var dynamicObject = serializer.Deserialize<dynamic>(reader);
                if (dynamicObject == null)
                {
                    return null;
                }

                rawValue = dynamicObject.value?.Value;
            }

            if (rawValue == null)
            {
                var value = GetDefault(valueType);
                return parseValueMethod.Invoke(null, new object[] { value });
            }
            else
            {
                try
                {
                    var value = Convert.ChangeType(rawValue, valueType, CultureInfo.InvariantCulture);
                    var result = parseValueMethod.Invoke(null, new object[] { value });
                    return result;
                }
                catch (FormatException)
                {
                    var parseMethod = GetParseMethod(objectType);
                    var result = parseMethod.Invoke(null, new object[] { rawValue });
                    return result;
                }
            }
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dynamicValue = value as dynamic;
            writer.WriteValue(dynamicValue.Value);
        }

        private static object GetDefault(Type t)
        {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t));
            }

            if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
            {
                return Activator.CreateInstance(t);
            }
            else
            {
                return null;
            }
        }

        private static MethodInfo GetParseValueMethod(Type type)
        {
            return parseValueMethodCache.GetOrAdd(type, _ => type.GetMethod("ParseValueOrCreate", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy));
        }

        private static MethodInfo GetParseMethod(Type type)
        {
            return parseMethodCache.GetOrAdd(type, _ => type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy));
        }

        private Type GetTypeOfValueParameter(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ExtendableEnumBase<,>))
            {
                var args = type.GetGenericArguments();
                return args[1];
            }

            if (type.BaseType != typeof(object))
            {
                return GetTypeOfValueParameter(type.BaseType);
            }

            return typeof(object);
        }
    }
}
