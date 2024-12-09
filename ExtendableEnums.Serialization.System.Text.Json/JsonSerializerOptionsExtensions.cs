using System.Text.Json.Serialization;

namespace ExtendableEnums.Serialization.System.Text.Json;

public static class JsonSerializerOptionsExtensions
{
    /// <summary>
    /// Adds custom converters required for serializing and deserializing extendable enums
    /// to the specified list of JSON converters.
    /// </summary>
    /// <example>
    /// Example usage:
    /// <code>
    /// var options = new JsonSerializerOptions
    /// {
    ///     PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    /// };
    ///
    /// // Add the custom converters
    /// options.Converters.AddExtendableEnums();
    ///
    /// var json = JsonSerializer.Serialize(yourObject, options);
    /// var obj = JsonSerializer.Deserialize&lt;YourType&gt;(json, options);
    /// </code>
    /// </example>
    public static void AddExtendableEnums(this IList<JsonConverter> converters)
    {
        converters.Add(new ExtendableEnumJsonConverter());
        converters.Add(new ExtendableEnumDictionaryJsonConverter());
    }
}