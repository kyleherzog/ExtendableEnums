﻿using ExtendableEnums.EntityFrameworkCore.UnitTests.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.EntityFrameworkCore.UnitTests.ModelBuilderExtensionsTests;

[TestClass]
public class ApplyExtendableEnumConversionsShould
{
    [TestMethod]
    public void ApplyTheValueConversions()
    {
        var seedCount = 1;
        using var context = DbContextFactory.Generate(seedCount);
        var people = context.Set<SamplePersonEntity>().ToList();

        people.Should().BeEquivalentTo(context.SeedData);
    }

    [TestMethod]
    public void NotApplyTheValueConversionsGivenIgnoredModelBuilder()
    {
        var seedCount = 1;
        using var context = DbContextFactory.Generate(seedCount);
        var entityProperties = context.Model.FindEntityType(typeof(SamplePersonEntity))!.GetProperties();
        var ignoredStatusProperty = entityProperties.FirstOrDefault(p => p.Name == nameof(SamplePersonEntity.IgnoredStatus));
        Assert.IsNull(ignoredStatusProperty);

        var statusProperty = entityProperties.FirstOrDefault(p => p.Name == nameof(SamplePersonEntity.Status));
        Assert.IsNotNull(statusProperty);
    }

    [TestMethod]
    public void NotApplyTheValueConversionsGivenNotMappedAttribute()
    {
        var seedCount = 1;
        using var context = DbContextFactory.Generate(seedCount);
        var entityProperties = context.Model.FindEntityType(typeof(SamplePersonEntity))!.GetProperties();
        var notMappedStatusProperty = entityProperties.FirstOrDefault(p => p.Name == nameof(SamplePersonEntity.NotMapppedStatus));
        Assert.IsNull(notMappedStatusProperty);

        var statusProperty = entityProperties.FirstOrDefault(p => p.Name == nameof(SamplePersonEntity.Status));
        Assert.IsNotNull(statusProperty);
    }
}