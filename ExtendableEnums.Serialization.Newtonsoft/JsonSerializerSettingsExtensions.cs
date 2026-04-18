using Newtonsoft.Json;

namespace ExtendableEnums.Serialization.Newtonsoft;

/// <summary>
/// Provides extension methods for <see cref="JsonSerializerSettings"/>.
/// </summary>
public static class JsonSerializerSettingsExtensions
{
    /// <summary>
    /// Configures the settings to use ExtendableEnum JSON converters via a contract resolver.
    /// </summary>
    /// <param name="settings">The JSON serializer settings to configure.</param>
    /// <returns>The configured <see cref="JsonSerializerSettings"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="settings"/> is null.</exception>
    public static JsonSerializerSettings UseExtendableEnums(this JsonSerializerSettings settings)
    {
        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        settings.ContractResolver = new ExtendableEnumContractResolver();
        return settings;
    }
}

