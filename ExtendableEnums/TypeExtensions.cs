namespace ExtendableEnums;

/// <summary>
/// Provides extension method on <see cref="Type"/> that relate to ExtendableEnums.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Gets the default value of the given type.
    /// </summary>
    /// <param name="type">The type of which to get the default value.</param>
    /// <returns>The default value of the given type.</returns>
    public static object? GetDefault(this Type type)
    {
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        object? output = null;

        if (type.IsValueType)
        {
            output = Activator.CreateInstance(type);
        }

        return output;
    }

    /// <summary>
    /// Checks to see if the given type is derived from <see cref="ExtendableEnumBase{TEnumeration, TValue}" />.
    /// </summary>
    /// <param name="type">The <see cref="Type" /> to check to see if it is an <see cref="ExtendableEnumBase{TEnumeration, TValue}"/> decendant.</param>
    /// <returns><c>true</c> if the type inherits from <see cref="ExtendableEnumBase{TEnumeration, TValue}"/>, otherwise <c>false</c>.</returns>
    public static bool IsExtendableEnum(this Type type)
    {
        return IsTypeDerivedFromGenericType(type, typeof(ExtendableEnumBase<,>));
    }

    /// <summary>
    /// Checks to see if the given type is derived from <see cref="ExtendableEnumDictionary{TKey, TValue}"/>.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to check.</param>
    /// <returns><c>true</c> if the type inherits from <see cref="ExtendableEnumDictionary{TKey, TValue}"/>, otherwise <c>false</c>.</returns>
    public static bool IsExtendableEnumDictionary(this Type type)
    {
        return IsTypeDerivedFromGenericType(type, typeof(ExtendableEnumDictionary<,>));
    }

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

    private static bool IsTypeDerivedFromGenericType(Type typeToCheck, Type genericType)
    {
        if (typeToCheck == typeof(object))
        {
            return false;
        }
        else if (typeToCheck is null)
        {
            return false;
        }
        else if (typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }
        else
        {
            return IsTypeDerivedFromGenericType(typeToCheck.BaseType, genericType);
        }
    }
}