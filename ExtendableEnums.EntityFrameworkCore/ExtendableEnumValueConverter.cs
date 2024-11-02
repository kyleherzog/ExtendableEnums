using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ExtendableEnums.EntityFrameworkCore;

/// <summary>
/// A static class for generating  <see cref="ValueConverter"/> instances that convert <see cref="ExtendableEnum{TEnumeration}" /> entity properties.
/// </summary>
public static class ExtendableEnumValueConverter
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> genericCreateMethodCache = new();
    private static readonly Lazy<MethodInfo> lazyCreateMethod = new(() => typeof(ExtendableEnumValueConverter).GetMethods(BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public).First(m => m.Name == nameof(Create) && m.ContainsGenericParameters));

    /// <summary>
    /// Creates a <see cref="ValueConverter"/> that converts the given ExtendableEnum type to/from an int.
    /// </summary>
    /// <param name="type">An <see cref="ExtendableEnum{TEnumeration}"/> type for which to convert to/from.</param>
    /// <returns>A <see cref="ValueConverter"/> that converts the given ExtendableEnum type to/from an int.</returns>
    public static ValueConverter Create(Type type)
    {
        var genericMethod = GetGenericCreateMethod(type);
        var result = genericMethod.Invoke(null, Array.Empty<object>()) as ValueConverter;

        return result
            ?? throw new ArgumentException($"Unable to create ValueConverter for type '{type.Name}'.", nameof(type));
    }

    /// <summary>
    /// Creates a <see cref="ValueConverter"/> that converts the given ExtendableEnum type to/from an int.
    /// </summary>
    /// <typeparam name="T">An <see cref="ExtendableEnum{TEnumeration}"/> type for which to convert to/from.</typeparam>
    /// <returns>A <see cref="ValueConverter"/> that converts the given ExtendableEnum type to/from an int.</returns>
    public static ValueConverter Create<T>()
        where T : ExtendableEnum<T>
    {
        var parseMethod = typeof(T).GetMethod("ParseValueOrCreate", BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public);

        if (parseMethod is null)
        {
            throw new MissingMethodException(typeof(T).Name, "ParseValueOrCreate");
        }

        return new ValueConverter<T, int>(x => x.Value, x => (T)parseMethod.Invoke(null, new object[] { x })!);
    }

    private static MethodInfo GetGenericCreateMethod(Type type)
    {
        var result = genericCreateMethodCache.GetOrAdd(type, t => lazyCreateMethod.Value.MakeGenericMethod(t));
        return result;
    }
}