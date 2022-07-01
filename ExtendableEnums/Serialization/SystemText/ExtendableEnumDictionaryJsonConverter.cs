using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using ExtendableEnums.Internals;

namespace ExtendableEnums.Serialization.SystemText;

/// <summary>
/// Converts ExtendableEnum objects to and from JSON.
/// </summary>
public class ExtendableEnumDictionaryJsonConverter : JsonConverterFactory
{
    /// <inheritdoc/>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType
            && typeToConvert.GetGenericTypeDefinition() == typeof(ExtendableEnumDictionary<,>);
    }

    /// <inheritdoc/>
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

        var argTypes = typeToConvert.GetGenericArguments();

        var converter = (JsonConverter)Activator.CreateInstance(
                typeof(EnumDictionaryConverterInner<,>).MakeGenericType(argTypes));

        return converter;
    }

    private sealed class EnumDictionaryConverterInner<TKey, TValue> : JsonConverter<ExtendableEnumDictionary<TKey, TValue?>>
            where TKey : IExtendableEnum
    {
        public override ExtendableEnumDictionary<TKey, TValue?>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var clonedOptions = new JsonSerializerOptions(options);
            clonedOptions.Converters.Clear();

            var result = (ExtendableEnumDictionary<TKey, TValue?>)Activator.CreateInstance(typeToConvert);

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return result;
                }

                var keyName = reader.GetString()
                    ?? throw new JsonException("Unable to deserialize the dictionary key.");
                var key = JsonSerializer.Deserialize<TKey>(keyName, clonedOptions)
                    ?? throw new JsonException($"Unable to deserialize the dictionary key as {typeof(TKey)}");
                reader.Read();
                var value = JsonSerializer.Deserialize<TValue>(ref reader, clonedOptions);

                result.Add(key, value);
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, ExtendableEnumDictionary<TKey, TValue?> value, JsonSerializerOptions options)
        {
            var clonedOptions = new JsonSerializerOptions(options);
            clonedOptions.Converters.Clear();

            writer.WriteStartObject();
            foreach (var key in value.Keys)
            {
                var keySerialized = JsonSerializer.Serialize(key);
                writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(keySerialized) ?? keySerialized);
                JsonSerializer.Serialize(writer, value[key], clonedOptions);
            }

            writer.WriteEndObject();
        }
    }
}