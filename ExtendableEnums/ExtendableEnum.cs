namespace ExtendableEnums;

/// <summary>
/// An <see cref="ExtendableEnumBase{TEnumeration, TValue}"/> that has its value type set as an <see cref="int"/>.
/// </summary>
/// <typeparam name="TEnumeration">The type of this enumeration (itself).</typeparam>
public abstract class ExtendableEnum<TEnumeration> : ExtendableEnumBase<TEnumeration, int>
    where TEnumeration : ExtendableEnumBase<TEnumeration, int>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExtendableEnum{TEnumeration}"/> class.
    /// </summary>
    /// <param name="value">The unique value that represents this enumeration value.</param>
    /// <param name="displayName">The <see cref="string"/> value that represents its display name.</param>
    [System.Text.Json.Serialization.JsonConstructor]
    protected ExtendableEnum(int value, string displayName)
        : base(value, displayName)
    {
    }
}