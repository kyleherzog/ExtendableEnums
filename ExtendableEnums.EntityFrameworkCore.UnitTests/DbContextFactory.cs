using ExtendableEnums.EntityFrameworkCore.UnitTests.Entities;

namespace ExtendableEnums.EntityFrameworkCore.UnitTests;

public static class DbContextFactory
{
    private const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=ExtendableEnumTests;Trusted_Connection=True;";

    public static TestingContext Generate(int seedCount)
    {
        SamplePersonEntityFactory.Initialize();

        var context = new TestingContext(connectionString)
        {
            SeedData = SamplePersonEntityFactory.Generate(seedCount).ToArray(),
        };

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }
}