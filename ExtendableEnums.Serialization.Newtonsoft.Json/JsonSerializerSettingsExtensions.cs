namespace Newtonsoft.Json;

public static class JsonSerializerSettingsExtensions
{
    /// <summary>
    /// Adds custom converters required for serializing and deserializing extendable enums
    /// to the specified list of JSON converters.
    /// </summary>
    /// <example>
    /// Example usage:
    /// <code>
    /// var settings = new JsonSerializerSettings
    /// {
    ///     Formatting = Formatting.Indented
    /// };
    ///
    /// // Add the custom converters
    /// settings.Converters.AddExtendableEnums();
    ///
    /// var json = JsonConvert.SerializeObject(yourObject, settings);
    /// var obj = JsonConvert.DeserializeObject&lt;YourType&gt;(json, settings);
    /// </code>
    /// </example>
    public static void AddExtendableEnums(this IList<JsonConverter> converters)
    {
        converters.Add(new ExtendableEnumJsonConverter());
        converters.Add(new ExtendableEnumDictionaryJsonConverter());
    }
}