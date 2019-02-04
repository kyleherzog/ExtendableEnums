using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace ExtendableEnums
{
    public class ExtendableEnumTypeConverter : TypeConverter
    {
        private static readonly ConcurrentDictionary<Type, MethodInfo> ParseMethodCache = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, MethodInfo> ParseValueMethodCache = new ConcurrentDictionary<Type, MethodInfo>();
        private Type enumerationType;
        private Type valueType;

        public ExtendableEnumTypeConverter(Type type)
        {
            if (!FillExtendedEnumTypeInfo(type))
            {
                throw new ArgumentException("Incompatible type", nameof(type));
            }
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var parseMethod = ParseMethodCache.GetOrAdd(enumerationType, t => t.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy));

                try
                {
                    var result = parseMethod.Invoke(null, new object[] { value });
                    return result;
                }
                catch (TargetInvocationException parseException) when (parseException.InnerException?.GetType() == typeof(ArgumentException))
                {
                    try
                    {
                        if (valueType == typeof(string))
                        {
                            var parseValueMethod = ParseValueMethodCache.GetOrAdd(enumerationType, t => t.GetMethod("ParseValue", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy));

                            var result = parseValueMethod.Invoke(null, new object[] { value });
                            return result;
                        }
                    }
                    catch (TargetInvocationException parseValueException) when (parseValueException.InnerException?.GetType() == typeof(ArgumentException))
                    {
                        throw parseValueException.InnerException;
                    }

                    throw parseException.InnerException;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        private bool FillExtendedEnumTypeInfo(Type type)
        {
            var currentType = type;
            while (currentType != typeof(object))
            {
                if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(ExtendableEnumBase<,>))
                {
                    var arguments = currentType.GetGenericArguments();
                    enumerationType = arguments[0];
                    valueType = arguments[1];
                    return true;
                }

                currentType = currentType.BaseType;
            }

            return false;
        }
    }
}
