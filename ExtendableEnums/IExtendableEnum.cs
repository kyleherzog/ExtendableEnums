namespace ExtendableEnums
{
    /// <summary>
    /// Base interface for <see cref="ExtendableEnumBase{TEnumeration, TValue}"/>, providing access to DisplayName.
    /// </summary>
    public interface IExtendableEnum
    {
        /// <summary>
        /// Gets the display name of the enumeration entry.
        /// </summary>
        string DisplayName { get; }
    }
}
