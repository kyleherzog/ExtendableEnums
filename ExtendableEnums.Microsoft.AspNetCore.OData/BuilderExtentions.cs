using System;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.AspNet.OData.Builder;

namespace ExtendableEnums.Microsoft.AspNetCore.OData
{
    public static class BuilderExtentions
    {
        public static ComplexTypeConfiguration AddExtendableEnum<TEnumeration>(this ODataConventionModelBuilder builder)
            where TEnumeration : IExtendableEnum
        {
            return AddExtendableEnum(builder, typeof(TEnumeration));
        }

        public static ComplexTypeConfiguration AddExtendableEnum(this ODataConventionModelBuilder builder, Type enumerationType)
        {
            var result = builder.AddComplexType(enumerationType);
            AddProperty(result, "Value");
            return result;
        }

        private static void AddProperty(StructuralTypeConfiguration config, string propertyName)
        {
            var propertyInfo = config.ClrType.GetProperty(propertyName);
            var property = config.AddProperty(propertyInfo);

            var attribute = propertyInfo.GetCustomAttribute<DataMemberAttribute>(inherit: false);

            if (attribute != null && !String.IsNullOrWhiteSpace(attribute.Name))
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
}
