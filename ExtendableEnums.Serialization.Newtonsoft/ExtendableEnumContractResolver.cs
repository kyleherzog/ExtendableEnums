using ExtendableEnums.Core;
using Newtonsoft.Json.Serialization;

namespace ExtendableEnums.Serialization.Newtonsoft;

/// <summary>
/// A contract resolver that automatically applies the appropriate JSON converters for ExtendableEnum types.
/// </summary>
public class ExtendableEnumContractResolver : DefaultContractResolver
{
    /// <summary>
    /// Resolves the contract converter for the specified object type.
    /// </summary>
    /// <param name="objectType">Type of the object.</param>
    /// <returns>The <see cref="global::Newtonsoft.Json.JsonConverter"/> for the specified object type.</returns>
    protected override global::Newtonsoft.Json.JsonConverter? ResolveContractConverter(Type objectType)
    {
        if (objectType is null)
        {
            throw new ArgumentNullException(nameof(objectType));
        }

        if (objectType.IsExtendableEnum())
        {
            return new ExtendableEnumJsonConverter();
        }

        if (objectType.IsExtendableEnumDictionary())
        {
            return new ExtendableEnumDictionaryJsonConverter();
        }

        return base.ResolveContractConverter(objectType);
    }
}

