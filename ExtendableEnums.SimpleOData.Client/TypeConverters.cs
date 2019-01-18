using System;

namespace ExtendableEnums.SimpleOData.Client
{
    public static class TypeConverters
    {
        public static ExtendableEnumBase<TEnumeration, TValue> ConvertExtendableEnum<TEnumeration, TValue>(object input)
            where TEnumeration : ExtendableEnumBase<TEnumeration, TValue>
            where TValue : IComparable
        {
            return ExtendableEnumBase<TEnumeration, TValue>.ParseValue((TValue)input);
        }
    }
}
