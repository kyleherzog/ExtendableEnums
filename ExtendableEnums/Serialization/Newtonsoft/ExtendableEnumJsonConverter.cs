using System.Diagnostics;
using System.Globalization;
using ExtendableEnums.Internals;
using Newtonsoft.Json;

namespace ExtendableEnums.Serialization.Newtonsoft;

/// <summary>
/// Converts ExtendableEnum objects to and from JSON.
/// </summary>
public class ExtendableEnumJsonConverter : JsonConverter
{
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

        var valueType = objectType.GetExtendableEnumArgs()[1];

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

        var parseValueOrCreateMethod = Methods.GetParseValueOrCreate(objectType);

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

                var tryParseValueMethod = Methods.GetTryParseValue(objectType);

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
                var tryParseMethod = Methods.GetTryParse(objectType);
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
}