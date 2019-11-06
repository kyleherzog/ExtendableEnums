using System.Linq;
using ExtendableEnums.EntityFrameworkCore.UnitTests.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.EntityFrameworkCore.UnitTests.ModelBuilderExtensionsTests
{
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
    }
}
