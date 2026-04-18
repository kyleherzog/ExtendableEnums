using System.Text.Json.Serialization;
using ExtendableEnums.Core;

namespace ExtendableEnums.Serialization.SystemText;

/// <summary>
/// A specialized dictionary to ensure proper serialization when an <see cref="ExtendableEnumBase{TEnumeration, TValue}" />
/// type object is the key type.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
[JsonConverter(typeof(ExtendableEnumDictionaryJsonConverter))]
public class SerializableExtendableEnumDictionary<TKey, TValue> : ExtendableEnumDictionary<TKey, TValue>
        where TKey : IExtendableEnum
{
}