using System;

namespace ExtendableEnums
{
    /// <summary>
    /// An interface extending <see cref="IExtendableEnum"/> which also exposes the value property.
    /// </summary>
    /// <typeparam name="TValue">The <see cref="Type"/> of the value property.</typeparam>
    public interface IExtendableEnum<out TValue> : IExtendableEnum
        where TValue : IComparable
    {
        /// <summary>
        /// Gets the unique value which represents this enumeration entry.
        /// </summary>
        TValue Value { get; }
    }
}
