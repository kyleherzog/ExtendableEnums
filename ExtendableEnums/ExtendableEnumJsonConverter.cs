using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Newtonsoft.Json;

namespace ExtendableEnums;

/// <summary>
/// Converts ExtendableEnum objects to and from JSON.
/// </summary>
public class ExtendableEnumJsonConverter : JsonConverter
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> tryParseValueMethodCache = new();

    private static readonly ConcurrentDictionary<Type, MethodInfo> parseValueOrCreateMethodCache = new();
    private static readonly ConcurrentDictionary<Type, MethodInfo> tryParseMethodCache = new();

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
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader is null)
        {
            throw new ArgumentNullException(nameof(reader));
        }

        if (objectType is null)
        {
            throw new ArgumentNullException(nameof(objectType));
        }

        if (serializer is null)
        {
            throw new ArgumentNullException(nameof(serializer));
        }

        var valueType = GetTypeOfValueParameter(objectType);

        var rawValue = reader.Value;
        if (rawValue is null)
        {
            var dynamicObject = serializer.Deserialize<dynamic>(reader);
            if (dynamicObject is null)
            {
                return null;
            }

            rawValue = dynamicObject.value?.Value;
        }

        var parseValueOrCreateMethod = GetParseValueOrCreateMethod(objectType);

        if (rawValue is null)
        {
            var value = GetDefault(valueType);
            return parseValueOrCreateMethod.Invoke(null, new object?[] { value });
        }
        else
        {
            try
            {
                var value = Convert.ChangeType(rawValue, valueType, CultureInfo.InvariantCulture);

                var tryParseValueMethod = GetTryParseValueMethod(objectType);

                var parameters = new object?[] { value, null };
                if ((bool)tryParseValueMethod.Invoke(null, parameters))
                {
                    return parameters[1];
                }
            }
            catch (FormatException)
            {
                Debug.WriteLine("FormatException caught.");
            }

            if (rawValue is string)
            {
                var rawParameters = new object?[] { rawValue, null };
                var tryParseMethod = GetTryParseMethod(objectType);
                if ((bool)tryParseMethod.Invoke(null, rawParameters))
                {
                    return rawParameters[1];
                }
            }

            var convertedValue = Convert.ChangeType(rawValue, valueType, CultureInfo.InvariantCulture);
            var result = parseValueOrCreateMethod.Invoke(null, new object[] { convertedValue });
            return result;
        }
    }

    /// <summary>
    /// Writes the JSON representation of the object.
    /// </summary>
    /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
    /// <param name="value">The value.</param>
    /// <param name="serializer">The calling serializer.</param>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var dynamicValue = value as dynamic;
        writer.WriteValue(dynamicValue?.Value);
    }

    private static object? GetDefault(Type t)
    {
        if (t is null)
        {
            throw new ArgumentNullException(nameof(t));
        }

        if (t.IsValueType && Nullable.GetUnderlyingType(t) is null)
        {
            return Activator.CreateInstance(t);
        }
        else
        {
            return null;
        }
    }

    private static MethodInfo GetTryParseValueMethod(Type type)
    {
        return tryParseValueMethodCache.GetOrAdd(type, _ => type.GetMethod("TryParseValue", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy));
    }

    private static MethodInfo GetParseValueOrCreateMethod(Type type)
    {
        return parseValueOrCreateMethodCache.GetOrAdd(type, _ => type.GetMethod("ParseValueOrCreate", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy));
    }

    private static MethodInfo GetTryParseMethod(Type type)
    {
        return tryParseMethodCache.GetOrAdd(type, _ => type.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy));
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