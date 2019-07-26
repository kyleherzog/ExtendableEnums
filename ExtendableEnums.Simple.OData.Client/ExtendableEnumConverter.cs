using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Simple.OData.Client;

namespace ExtendableEnums.SimpleOData.Client
{
    /// <summary>
    /// A static class that provides helper methods to allows the Simple OData client to serialize/deserialize ExtendableEnums.
    /// </summary>
    public static class ExtendableEnumConverter
    {
        private static readonly ConcurrentDictionary<Type, MethodInfo> genericConvertMethodCache = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly Lazy<MethodInfo> primaryConvertMethod = new Lazy<MethodInfo>(() => typeof(ExtendableEnumConverter).GetMethod(nameof(Convert), BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof(IDictionary<string, object>) }, null));
        private static readonly ConcurrentDictionary<Type, TypeConverterConfiguration> typeConfigurationCache = new ConcurrentDictionary<Type, TypeConverterConfiguration>();

        /// <summary>
        /// Registers type converter with the ODataSettings to be used with an ODataClient.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type" /> to be registered as an ExtenableEnum.</typeparam>
        /// <param name="settings">The <see cref="ODataClientSettings" /> with which to register the type converter.</param>
        public static void Register<T>(ODataClientSettings settings)
            where T : IExtendableEnum
        {
            Register(typeof(T), settings);
        }

        /// <summary>
        /// Registers type converter with the ODataSettings to be used with an ODataClient.
        /// </summary>
        /// <param name="enumDescendant">The <see cref="Type" /> to be registered as an ExtenableEnum.</param>
        /// <param name="settings">The <see cref="ODataClientSettings" /> with which to register the type converter.</param>
        public static void Register(Type enumDescendant, ODataClientSettings settings)
        {
            if (enumDescendant == null)
            {
                throw new ArgumentNullException(nameof(enumDescendant));
            }

            var config = typeConfigurationCache.GetOrAdd(enumDescendant, GetTypeConverterConfiguration(enumDescendant));
            if (config == null)
            {
                throw new ArgumentException($"The type argument must inherit from ExtendableEnumBase.", nameof(enumDescendant));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.TypeCache.Converter.RegisterTypeConverter(enumDescendant, d =>
            {
                return Convert(config, d);
            });
        }

        /// <summary>
        /// Converts a dictionary with a value key to an ExtendableEnum of equivilent value.
        /// </summary>
        /// <param name="enumerationType">The ExtendableEnum descendant type to convert to.</param>
        /// <param name="valueType">The <see cref="Type" /> of the value.</param>
        /// <param name="dictionary">The dictionary containing the Value key to use to convert to an ExtenableEnum.</param>
        /// <returns>A dynamic object which has an underlying type of the given the <see cref="Type"/> specied, set
        /// to the value found in the dictionary parameter passed in.</returns>
        internal static dynamic Convert(Type enumerationType, Type valueType, IDictionary<string, object> dictionary)
        {
            var genericMethod = genericConvertMethodCache.GetOrAdd(enumerationType, _ => primaryConvertMethod.Value.MakeGenericMethod(enumerationType, valueType));
            return genericMethod.Invoke(null, new object[] { dictionary });
        }

        private static ExtendableEnumBase<TEnumeration, TValue> Convert<TEnumeration, TValue>(IDictionary<string, object> dictionary)
                                    where TEnumeration : ExtendableEnumBase<TEnumeration, TValue>
                                    where TValue : IComparable
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            var type = typeof(TValue);
            dynamic typedValue = System.Convert.ChangeType(dictionary["value"], type, CultureInfo.InvariantCulture);

            return ExtendableEnumBase<TEnumeration, TValue>.ParseValue(typedValue);
        }

        private static dynamic Convert(TypeConverterConfiguration config, IDictionary<string, object> dictionary)
        {
            return Convert(config.EnumerationType, config.ValueType, dictionary);
        }

        private static TypeConverterConfiguration GetTypeConverterConfiguration(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ExtendableEnumBase<,>))
            {
                var args = type.GetGenericArguments();
                return new TypeConverterConfiguration
                {
                    EnumerationType = args[0],
                    ValueType = args[1],
                };
            }

            if (type.BaseType != typeof(object))
            {
                return GetTypeConverterConfiguration(type.BaseType);
            }

            return null;
        }

        private class TypeConverterConfiguration
        {
            public Type EnumerationType { get; set; }

            public Type ValueType { get; set; }
        }
    }
}
