using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using Newtonsoft.Json;

namespace ExtendableEnums
{
    public class ExtendableEnumJsonConverter : JsonConverter
    {
        private static readonly ConcurrentDictionary<Type, MethodInfo> parseValueMethodCache = new ConcurrentDictionary<Type, MethodInfo>();

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var valueType = GetTypeOfValueParameter(objectType);

            var method = GetParseValueMethod(objectType);
            var value = Convert.ChangeType(reader.Value, valueType, CultureInfo.InvariantCulture);
            var result = method.Invoke(null, new object[] { value });
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dynamicValue = value as dynamic;
            writer.WriteValue(dynamicValue.Value);
        }

        private static MethodInfo GetParseValueMethod(Type type)
        {
            return parseValueMethodCache.GetOrAdd(type, _ => type.GetMethod("ParseValue", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy));
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
