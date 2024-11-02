using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace ExtendableEnums;

/// <summary>
/// Provides a way of converting ExtendableEnums to/from other predefined types.
/// </summary>
public class ExtendableEnumTypeConverter : TypeConverter
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> parseMethodCache = new();
    private static readonly ConcurrentDictionary<Type, MethodInfo> parseValueMethodCache = new();
    private Type enumerationType = typeof(object);
    private Type valueType = typeof(object);

    /// <summary>
    /// Initializes a new instance of the <see cref="ExtendableEnumTypeConverter"/> class.
    /// </summary>
    /// <param name="type">A type that extends ExtendableEnumBase.</param>
    public ExtendableEnumTypeConverter(Type type)
    {
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        if (!FillExtendedEnumTypeInfo(type))
        {
            throw new ArgumentException("Incompatible type", nameof(type));
        }
    }

    /// <summary>
    /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
    /// <param name="sourceType">A <see cref="Type"/> that represents the type you want to convert from.</param>
    /// <returns><c>true</c> if this converter can perform the conversion; otherwise, <c>false</c>.</returns>
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        if (sourceType == typeof(string))
        {
            return true;
        }

        return base.CanConvertFrom(context, sourceType);
    }

    /// <summary>
    /// Returns whether this converter can convert the object to the specified type.
    /// </summary>
    /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
    /// <param name="destinationType">A <see cref="Type"/> that represents the type you want to convert from.</param>
    /// <returns><c>true</c> if this converter can perform the conversion; otherwise, <c>false</c>.</returns>
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
        if (destinationType == typeof(string))
        {
            return true;
        }

        return base.CanConvertTo(context, destinationType);
    }

    /// <summary>
    /// Converts the given value to the type of this converter.
    /// </summary>
    /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
    /// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
    /// <param name="value">The <see cref="Object"/> to convert.</param>
    /// <returns>An <see cref="Object"/> that represents the converted value.</returns>
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string)
        {
            var parseMethod = parseMethodCache.GetOrAdd(enumerationType, t => t.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy));

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
                        var parseValueMethod = parseValueMethodCache.GetOrAdd(enumerationType, t => t.GetMethod("ParseValueOrCreate", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy));

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