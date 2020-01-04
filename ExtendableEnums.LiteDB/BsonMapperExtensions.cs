using System;
using System.Reflection;
using LiteDB;

namespace ExtendableEnums.LiteDB
{
    /// <summary>
    /// Provides extension methods for mapping ExtendableEnums in LiteDB.
    /// </summary>
    public static class BsonMapperExtensions
    {
        /// <summary>
        /// Registers mappings for all ExtendedEnums in a given assembly that have a Value property type of Int32.
        /// </summary>
        /// <param name="mapper">The <see cref="BsonMapper"/> for which the mapping will be registered.</param>
        /// <param name="assembly">The assembly to search for ExtendableEnums.</param>
        public static void RegisterAllInt32BasedExtendableEnums(this BsonMapper mapper, Assembly assembly)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.IsExtendableEnum())
                {
                    var valueProperty = type.GetProperty("Value");
                    if (valueProperty.PropertyType == typeof(int))
                    {
                        RegisterExtendableEnumAsInt32(mapper, type);
                    }
                }
            }
        }

        /// <summary>
        /// Registers mappings for all ExtendedEnums in an assembly containing a given type that have a Value property type of Int32.
        /// </summary>
        /// <param name="mapper">The <see cref="BsonMapper"/> for which the mapping will be registered.</param>
        /// <param name="assemblyMarkerType">A <see cref="Type"/> which is contained in an <see cref="Assembly"/> which is to be searched.</param>
        public static void RegisterAllInt32BasedExtendableEnums(this BsonMapper mapper, Type assemblyMarkerType)
        {
            if (assemblyMarkerType == null)
            {
                throw new ArgumentNullException(nameof(assemblyMarkerType));
            }

            RegisterAllInt32BasedExtendableEnums(mapper, assemblyMarkerType.Assembly);
        }

        /// <summary>
        /// Registers mappings for all ExtendedEnums in a given assembly that have a Value property type of string.
        /// </summary>
        /// <param name="mapper">The <see cref="BsonMapper"/> for which the mapping will be registered.</param>
        /// <param name="assembly">The assembly to search for ExtendableEnums.</param>
        public static void RegisterAllStringBasedExtendableEnums(this BsonMapper mapper, Assembly assembly)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.IsExtendableEnum())
                {
                    var valueProperty = type.GetProperty("Value");
                    if (valueProperty.PropertyType == typeof(string))
                    {
                        RegisterExtendableEnumAsString(mapper, type);
                    }
                }
            }
        }

        /// <summary>
        /// Registers mappings for all ExtendedEnums in an assembly containing a given type that have a Value property type of string.
        /// </summary>
        /// <param name="mapper">The <see cref="BsonMapper"/> for which the mapping will be registered.</param>
        /// <param name="assemblyMarkerType">A <see cref="Type"/> which is contained in an <see cref="Assembly"/> which is to be searched.</param>
        public static void RegisterAllStringBasedExtendableEnums(this BsonMapper mapper, Type assemblyMarkerType)
        {
            if (assemblyMarkerType == null)
            {
                throw new ArgumentNullException(nameof(assemblyMarkerType));
            }

            RegisterAllStringBasedExtendableEnums(mapper, assemblyMarkerType.Assembly);
        }

        /// <summary>
        /// Registers mappings for a specific ExtendableEnum type with a Value property type of Int32.
        /// </summary>
        /// <param name="mapper">The <see cref="BsonMapper"/> for which the mapping will be registered.</param>
        /// <param name="type">An ExtendableEnum <see cref="Type"/> which is to be registered.</param>
        public static void RegisterExtendableEnumAsInt32(this BsonMapper mapper, Type type)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.IsExtendableEnum())
            {
                throw new ArgumentException("The type must be an ExtendableEnum.", nameof(type));
            }

            var valueProperty = type.GetProperty("Value");
            if (valueProperty.PropertyType != typeof(int))
            {
                throw new ArgumentException("The ExtendableEnum type must have a value type of int.", nameof(type));
            }

            var parseMethod = type.GetMethod("ParseValueOrCreate", BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public);
            mapper.RegisterType(type, s => ((IExtendableEnum<int>)s).Value, b => parseMethod.Invoke(null, new object[] { b.AsInt32 }));
        }

        /// <summary>
        /// Registers mappings for a specific ExtendableEnum type with a Value property type of string.
        /// </summary>
        /// <param name="mapper">The <see cref="BsonMapper"/> for which the mapping will be registered.</param>
        /// <param name="type">An ExtendableEnum <see cref="Type"/> which is to be registered.</param>
        public static void RegisterExtendableEnumAsString(this BsonMapper mapper, Type type)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.IsExtendableEnum())
            {
                throw new ArgumentException("The type must be an ExtendableEnum.", nameof(type));
            }

            var valueProperty = type.GetProperty("Value");
            if (valueProperty.PropertyType != typeof(string))
            {
                throw new ArgumentException("The ExtendableEnum type must have a value type of string.", nameof(type));
            }

            var parseMethod = type.GetMethod("ParseValueOrCreate", BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public);
            mapper.RegisterType(type, s => ((IExtendableEnum<string>)s).Value, b => parseMethod.Invoke(null, new object[] { b.AsString }));
        }
    }
}
