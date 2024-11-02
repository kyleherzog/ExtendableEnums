using Microsoft.EntityFrameworkCore;

namespace ExtendableEnums.EntityFrameworkCore;

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
        if (modelBuilder is null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        var entityTypes = modelBuilder.Model.GetEntityTypes();
        foreach (var clrType in entityTypes.Select(x => x.ClrType))
        {
            var properties = clrType.GetProperties();
            foreach (var property in properties.Where(x => x.PropertyType.IsExtendableEnum()))
            {
                modelBuilder.Entity(clrType).Property(property.Name).HasConversion(ExtendableEnumValueConverter.Create(property.PropertyType));
            }
        }

        return modelBuilder;
    }
}