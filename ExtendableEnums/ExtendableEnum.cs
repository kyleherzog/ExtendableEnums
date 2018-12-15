using System;
using System.Collections.Generic;
using System.Text;

namespace ExtendableEnums
{
    public abstract class ExtendableEnum<TEnumeration> : ExtendableEnumBase<TEnumeration, int>
        where TEnumeration : ExtendableEnumBase<TEnumeration, int>
    {
        protected ExtendableEnum(int value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
