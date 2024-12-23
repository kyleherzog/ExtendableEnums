﻿using ExtendableEnums.EntityFrameworkCore.UnitTests.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace ExtendableEnums.EntityFrameworkCore.UnitTests;

public class TestingContext : DbContext
{
    public TestingContext(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public string ConnectionString { get; }

    public bool IsLoggingSensitiveData { get; set; }

    public ILoggerFactory? LoggerFactory { get; set; }

    public DbSet<SamplePersonEntity>? People { get; set; }

    public IEnumerable<object>? SeedData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder is null)
        {
            throw new ArgumentNullException(nameof(optionsBuilder));
        }

        optionsBuilder.UseSqlServer(ConnectionString);

        if (SeedData is not null)
        {
            optionsBuilder.ReplaceService<IModelCacheKeyFactory, NoModelCacheKeyFactory>();
        }

        optionsBuilder.UseLoggerFactory(LoggerFactory);
        if (IsLoggingSensitiveData)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder is null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        modelBuilder.Entity<SamplePersonEntity>().Ignore(x => x.IgnoredStatus);

        if (SeedData is not null)
        {
            var types = SeedData.Select(x => x.GetType()).Distinct();
            foreach (var type in types)
            {
                var entityBuilder = modelBuilder.Entity(type);
                var data = SeedData.Where(x => x.GetType() == type).ToArray();
                entityBuilder.HasData(data);
            }
        }

        modelBuilder.ApplyExtendableEnumConversions();

        base.OnModelCreating(modelBuilder);
    }
}