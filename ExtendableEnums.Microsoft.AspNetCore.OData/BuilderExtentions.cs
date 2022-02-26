using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.AspNet.OData.Builder;

namespace ExtendableEnums.Microsoft.AspNetCore.OData;

/// <summary>
/// Provides helper methods to register ExtendableEnums with the <see cref="ODataConventionModelBuilder" />.
/// </summary>
public static class BuilderExtentions
{
    /// <summary>
    /// Registers all types existing in the <see cref="Assembly"/> containing the given marker type, that inherit from <see cref="ExtendableEnumBase{TEnumeration, TValue}"/> with the <see cref="ODataConventionModelBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="ODataConventionModelBuilder"/> with which to register the ExtendableEnum.</param>
    /// <param name="assemblyMarkerType">The <see cref="Type"/> to use as a reference to find the containing <see cref="Assembly"/> that will be searched for ExtendableEnums to be registered.</param>
    /// <returns>The original <see cref="ODataConventionModelBuilder"/> which was passed in as a parameter.</returns>
    public static ODataConventionModelBuilder AddAllExtendableEnums(this ODataConventionModelBuilder builder, Type assemblyMarkerType)
    {
        if (assemblyMarkerType is null)
        {
            throw new ArgumentNullException(nameof(assemblyMarkerType));
        }

        return AddAllExtendableEnums(builder, assemblyMarkerType.Assembly);
    }

    /// <summary>
    /// Registers all types existing in the given <see cref="Assembly"/> that inherit from <see cref="ExtendableEnumBase{TEnumeration, TValue}"/> with the <see cref="ODataConventionModelBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="ODataConventionModelBuilder"/> with which to register the ExtendableEnum.</param>
    /// <param name="assembly">The <see cref="Assembly "/> in which to search for ExtendableEnums.</param>
    /// <returns>The original <see cref="ODataConventionModelBuilder"/> which was passed in as a parameter.</returns>
    public static ODataConventionModelBuilder AddAllExtendableEnums(this ODataConventionModelBuilder builder, Assembly assembly)
    {
        if (assembly is null)
        {
            throw new ArgumentNullException(nameof(assembly));
        }

        var types = assembly.GetTypes();
        foreach (var type in types.Where(x => x.IsExtendableEnum()))
        {
            AddExtendableEnum(builder, type);
        }

        return builder;
    }

    /// <summary>
    /// Registers an ExtendableEnum type with the <see cref="ODataConventionModelBuilder" />.
    /// </summary>
    /// <typeparam name="TEnumeration">The actual <see cref="Type"/> of the class that inherits from <see cref="ExtendableEnumBase{TEnumeration, TValue}" /> that is to be registered.</typeparam>
    /// <param name="builder">The <see cref="ODataConventionModelBuilder"/> with which to register the ExtendableEnum.</param>
    /// <returns>The <see cref="ComplexTypeConfiguration"/> that the ExtendableEnum gets registered as.</returns>
    public static ComplexTypeConfiguration AddExtendableEnum<TEnumeration>(this ODataConventionModelBuilder builder)
        where TEnumeration : IExtendableEnum
    {
        return AddExtendableEnum(builder, typeof(TEnumeration));
    }

    /// <summary>
    /// Registers an ExtendableEnum type with the <see cref="ODataConventionModelBuilder" />.
    /// </summary>
    /// <param name="builder">The <see cref="ODataConventionModelBuilder"/> with which to register the ExtendableEnum.</param>
    /// <param name="enumerationType">The actual <see cref="Type"/> of the class that inherits from <see cref="ExtendableEnumBase{TEnumeration, TValue}" /> that is to be registered.</param>
    /// <returns>The <see cref="ComplexTypeConfiguration"/> that the ExtendableEnum gets registered as.</returns>
    public static ComplexTypeConfiguration AddExtendableEnum(this ODataConventionModelBuilder builder, Type enumerationType)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        var result = builder.AddComplexType(enumerationType);
        AddProperty(result, "Value");
        return result;
    }

    private static void AddProperty(StructuralTypeConfiguration config, string propertyName)
    {
        var propertyInfo = config.ClrType.GetProperty(propertyName);
        var property = config.AddProperty(propertyInfo);

        var attribute = propertyInfo.GetCustomAttribute<DataMemberAttribute>(inherit: false);

        if (attribute is not null && !String.IsNullOrWhiteSpace(attribute.Name))
        {
            property.Name = attribute.Name;
        }
        else
        {
            var caser = new LowerCamelCaser();
            property.Name = caser.ToLowerCamelCase(propertyInfo.Name);
        }
    }
}