using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Simple.OData.Client;

namespace ExtendableEnums.SimpleOData.Client
{
    public static class ExtendableEnumConverter
    {
        private static ConcurrentDictionary<Type, TypeConverterConfiguration> typeConfigurationCache = new ConcurrentDictionary<Type, TypeConverterConfiguration>();
        private static ConcurrentDictionary<Type, MethodInfo> GenericConvertMethodCache = new ConcurrentDictionary<Type, MethodInfo>();
        private static Lazy<MethodInfo> primaryConvertMethod = new Lazy<MethodInfo>(() => typeof(ExtendableEnumConverter).GetMethod(nameof(Convert), new Type[] { typeof(IDictionary<string, object>) }));

        public static ExtendableEnumBase<TEnumeration, TValue> Convert<TEnumeration, TValue>(IDictionary<string, object> dictionary)
            where TEnumeration : ExtendableEnumBase<TEnumeration, TValue>
            where TValue : IComparable
        {
            Type type = typeof(TValue);
            dynamic typedValue = System.Convert.ChangeType(dictionary["value"], type);

            return ExtendableEnumBase<TEnumeration, TValue>.ParseValue(typedValue);
        }

        private static dynamic Convert(TypeConverterConfiguration config, IDictionary<string, object> dictionary)
        {
            return Convert(config.EnumerationType, config.ValueType, dictionary);
        }

        public static dynamic Convert(Type enumerationType, Type valueType, IDictionary<string, object> dictionary)
        {
            var genericMethod = GenericConvertMethodCache.GetOrAdd(enumerationType, _ => primaryConvertMethod.Value.MakeGenericMethod(enumerationType, valueType));
            return genericMethod.Invoke(null, new object[] { dictionary });
        }

        public static void Register<T>(ODataClientSettings settings)
            where T : IExtendableEnum
        {
            Register(typeof(T), settings);
        }

        public static void Register(Type enumDescendant, ODataClientSettings settings)
        {
            var config = typeConfigurationCache.GetOrAdd(enumDescendant, GetTypeConverterConfiguration(enumDescendant));
            if (config == null)
            {
                throw new ArgumentException($"The type argument must inherit from ExtendableEnumBase.", nameof(enumDescendant));
            }

            settings.TypeCache.Converter.RegisterTypeConverter(enumDescendant, d =>
            {
                return Convert(config, d);
            });
        }

        private static TypeConverterConfiguration GetTypeConverterConfiguration(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ExtendableEnumBase<,>))
            {
                var args = type.GetGenericArguments();
                return new TypeConverterConfiguration
                {
                    EnumerationType = args[0],
                    ValueType = args[1]
                };
            }

            if (type.BaseType != typeof(object))
            {
                return GetTypeConverterConfiguration(type.BaseType);
            }

            return null;
        }
    }

    internal class TypeConverterConfiguration
    {
        public Type ValueType { get; set; }

        public Type EnumerationType { get; set; }
    }
}
