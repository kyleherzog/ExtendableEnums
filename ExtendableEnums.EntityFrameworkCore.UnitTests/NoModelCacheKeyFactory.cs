using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ExtendableEnums.EntityFrameworkCore.UnitTests;

/// <summary>
/// Creates a unique cache key everytime to ensure no caching happens.
/// </summary>
public class NoModelCacheKeyFactory : IModelCacheKeyFactory
{
    public object Create(DbContext context)
    {
        return Guid.NewGuid();
    }
}