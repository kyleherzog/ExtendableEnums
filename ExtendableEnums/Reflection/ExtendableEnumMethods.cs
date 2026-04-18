using System.Collections.Concurrent;
using System.Reflection;

namespace ExtendableEnums.Core.Reflection;

/// <summary>
/// Provides means for retrieving method infomormation from ExtendableEnum objects.
/// </summary>
public static class ExtendableEnumMethods
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> parseValueOrCreateMethodCache = new();
    private static readonly ConcurrentDictionary<Type, MethodInfo> tryParseMethodCache = new();
    private static readonly ConcurrentDictionary<Type, MethodInfo> tryParseValueMethodCache = new();

    /// <summary>
    /// Gets the ParseValueOrCreate method.
    /// </summary>
    /// <param name="type">The ExtendableEnum based type.</param>
    /// <returns>The <see cref="MethodInfo"/> found.</returns>
    public static MethodInfo GetParseValueOrCreate(Type type)
    {
        return parseValueOrCreateMethodCache.GetOrAdd(type, t => t.GetMethod("ParseValueOrCreate", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy));
    }

    /// <summary>
    /// Gets the TryParse method.
    /// </summary>
    /// <param name="type">The ExtendableEnum based type.</param>
    /// <returns>The <see cref="MethodInfo"/> found.</returns>
    public static MethodInfo GetTryParse(Type type)
    {
        return tryParseMethodCache.GetOrAdd(type, t => t.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy));
    }

    /// <summary>
    /// Gets the TryParseValue method.
    /// </summary>
    /// <param name="type">The ExtendableEnum based type.</param>
    /// <returns>The <see cref="MethodInfo"/> found.</returns>
    public static MethodInfo GetTryParseValue(Type type)
    {
        return tryParseValueMethodCache.GetOrAdd(type, t => t.GetMethod("TryParseValue", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy));
    }
}