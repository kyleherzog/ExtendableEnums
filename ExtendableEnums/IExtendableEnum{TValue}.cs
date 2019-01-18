using System;
using System.Collections.Generic;
using System.Text;

namespace ExtendableEnums
{
    public interface IExtendableEnum<out TValue> : IExtendableEnum
        where TValue : IComparable
    {
        TValue Value { get; }
    }
}
