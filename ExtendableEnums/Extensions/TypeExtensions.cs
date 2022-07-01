using System;

namespace ExtendableEnums.Extensions;

internal static class TypeExtensions
{
    internal static Type[] GetExtendableEnumArgs(this Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ExtendableEnumBase<,>))
        {
            var args = type.GetGenericArguments();
            return args;
        }

        if (type.BaseType != typeof(object))
        {
            return GetExtendableEnumArgs(type.BaseType);
        }

        return Array.Empty<Type>();
    }
}