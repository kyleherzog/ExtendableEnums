using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace ExtendableEnums.Internals;

internal static class Methods
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> parseValueOrCreateMethodCache = new();
    private static readonly ConcurrentDictionary<Type, MethodInfo> tryParseMethodCache = new();
    private static readonly ConcurrentDictionary<Type, MethodInfo> tryParseValueMethodCache = new();

    internal static MethodInfo GetParseValueOrCreate(Type type)
    {
        return parseValueOrCreateMethodCache.GetOrAdd(type, _ => type.GetMethod("ParseValueOrCreate", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy));
    }

    internal static MethodInfo GetTryParse(Type type)
    {
        return tryParseMethodCache.GetOrAdd(type, _ => type.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy));
    }

    internal static MethodInfo GetTryParseValue(Type type)
    {
        return tryParseValueMethodCache.GetOrAdd(type, _ => type.GetMethod("TryParseValue", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy));
    }
}