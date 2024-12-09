using System.Collections.Concurrent;
using System.Reflection;

namespace ExtendableEnums.Serialization.System.Text.Json.Internals;

internal static class Methods
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> parseValueOrCreateMethodCache = new();

    internal static MethodInfo GetParseValueOrCreate(Type type)
    {
        return parseValueOrCreateMethodCache.GetOrAdd(type, t => t.GetMethod("ParseValueOrCreate", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy));
    }
}