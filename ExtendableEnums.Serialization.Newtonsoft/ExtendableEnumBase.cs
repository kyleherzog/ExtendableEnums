using Newtonsoft.Json;

namespace ExtendableEnums.Serialization.Newtonsoft;

/// <summary>
/// An <see cref="ExtendableEnums.Core.ExtendableEnumBase{TEnumeration, TValue}"/> with Newtonsoft.Json serialization
/// pre-configured via the <see cref="JsonConverterAttribute"/>.
/// Consumers may use this as a base class in place of <see cref="ExtendableEnums.Core.ExtendableEnumBase{TEnumeration, TValue}"/>
/// to avoid adding a <see cref="JsonConverterAttribute"/> to each enum class.
/// </summary>
/// <typeparam name="TEnumeration">The type of this enumeration (itself).</typeparam>
/// <typeparam name="TValue">The type of the value property.</typeparam>
[JsonConverter(typeof(ExtendableEnumJsonConverter))]
public abstract class ExtendableEnumBase<TEnumeration, TValue> : ExtendableEnums.Core.ExtendableEnumBase<TEnumeration, TValue>
    where TEnumeration : ExtendableEnums.Core.ExtendableEnumBase<TEnumeration, TValue>
    where TValue : IComparable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExtendableEnumBase{TEnumeration, TValue}"/> class.
    /// </summary>
    /// <param name="value">The unique value that represents this enumeration value.</param>
    /// <param name="displayName">The <see cref="string"/> value that represents its display name.</param>
    protected ExtendableEnumBase(TValue value, string displayName)
        : base(value, displayName)
    {
    }
}

