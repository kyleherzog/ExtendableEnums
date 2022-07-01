using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using ExtendableEnums.Extensions;
using ExtendableEnums.Internals;

namespace ExtendableEnums.Serialization.SystemText;

/// <summary>
/// Converts ExtendableEnum objects to and from JSON.
/// </summary>
public class ExtendableEnumJsonConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        var canConvert = typeToConvert.IsExtendableEnum();
        return canConvert;
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert == null)
        {
            throw new ArgumentNullException(nameof(typeToConvert));
        }

        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        var argTypes = typeToConvert.GetExtendableEnumArgs();

        var converter = (JsonConverter)Activator.CreateInstance(
                typeof(EnumConverterInner<,>).MakeGenericType(argTypes));

        return converter;
    }

    private sealed class EnumConverterInner<TEnumeration, TValue> : JsonConverter<TEnumeration>
        where TValue : IComparable
        where TEnumeration : IExtendableEnum<TValue>
    {
        public override TEnumeration? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = GetDefault(typeof(TValue));
            object? rawValue;
            var shouldReturnDefault = false;

            var element = JsonSerializer.Deserialize<JsonElement>(ref reader);
            if (element.ValueKind == JsonValueKind.Object)
            {
                if (!element.TryGetProperty("value", out element))
                {
                    shouldReturnDefault = true;
                }
            }

            if (!shouldReturnDefault)
            {
                if (element.ValueKind == JsonValueKind.String)
                {
                    rawValue = element.GetString();
                    value = Convert.ChangeType(rawValue, typeof(TValue), CultureInfo.InvariantCulture);
                }
                else
                {
                    value = element.Deserialize<TValue>();
                }
            }

            var parseValueOrCreateMethod = Methods.GetParseValueOrCreate(typeof(TEnumeration));
            var result = (TEnumeration)parseValueOrCreateMethod.Invoke(null, new object?[] { value });
            return result;
        }

        public override void Write(Utf8JsonWriter writer, TEnumeration value, JsonSerializerOptions options)
        {
            var clonedOptions = new JsonSerializerOptions(options);
            clonedOptions.Converters.Clear();
            JsonSerializer.Serialize(writer, value.Value, clonedOptions);
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
}