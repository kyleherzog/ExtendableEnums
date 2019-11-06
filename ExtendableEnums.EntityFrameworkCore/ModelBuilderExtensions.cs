using EnsureThat;
using Microsoft.EntityFrameworkCore;

namespace ExtendableEnums.EntityFrameworkCore
{
    /// <summary>
    /// Provides extension method for the <see cref="ModelBuilder"/>.
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Applies the value conversions for all <see cref="ExtendableEnum{TEnumeration}"/> based properties for all entities present on the given <see cref="ModelBuilder"/>.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> for which to apply the value conversions.</param>
        /// <returns>The <see cref="ModelBuilder"/> passed in.</returns>
        public static ModelBuilder ApplyExtendableEnumConversions(this ModelBuilder modelBuilder)
        {
            EnsureArg.IsNotNull(modelBuilder, nameof(modelBuilder));

            var entityTypes = modelBuilder.Model.GetEntityTypes();
            foreach (var entityType in entityTypes)
            {
                var properties = entityType.ClrType.GetProperties();
                foreach (var property in properties)
                {
                    if (property.PropertyType.IsExtendableEnum())
                    {
                        modelBuilder.Entity(entityType.ClrType).Property(property.Name).HasConversion(ExtendableEnumValueConverter.Create(property.PropertyType));
                    }
                }
            }

            return modelBuilder;
        }
    }
}
