using System;
using System.Collections;
using Newtonsoft.Json;

namespace ExtendableEnums
{
    /// <summary>
    /// Converts <see cref="ExtendableEnumDictionary{TKey, TValue}"/> objects to and from JSON.
    /// </summary>
    public class ExtendableEnumDictionaryJsonConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsExtendableEnumDictionary();
        }

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == null)
            {
                throw new ArgumentNullException(nameof(objectType));
            }

            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            if (!CanConvert(objectType))
            {
                throw new ArgumentException($"This converter is not for {objectType}.");
            }

            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var keyType = objectType.GetGenericArguments()[0];
            var valueType = objectType.GetGenericArguments()[1];
            var result = (IDictionary)Activator.CreateInstance(objectType);

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                {
                    return result;
                }

                var key = serializer.Deserialize(reader, keyType);
                reader.Read();
                var value = serializer.Deserialize(reader, valueType);
                result.Add(key, value);
            }

            return result;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var dictionary = (IDictionary)value;

            writer.WriteStartObject();
            foreach (var key in dictionary.Keys)
            {
                if (key is IExtendableEnum<string> stringEnum)
                {
                    writer.WritePropertyName(stringEnum.Value);
                }
                else
                {
                    var keySerialized = JsonConvert.SerializeObject(key);
                    writer.WritePropertyName(keySerialized);
                }

                serializer.Serialize(writer, dictionary[key]);
            }

            writer.WriteEndObject();
        }
    }
}
