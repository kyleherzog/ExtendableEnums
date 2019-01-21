using System;
using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests
{
    [TestClass]
    public class SortShould
    {
        [TestMethod]
        public void SortShouldOrderByValue()
        {
            var unordered = new[] { SampleStatus.Inactive, SampleStatus.Discontinued, SampleStatus.Active };
            var ordered = new[] { SampleStatus.Active, SampleStatus.Discontinued, SampleStatus.Inactive };

            Array.Sort(unordered);

            for (var i = 0; i < ordered.Length; i++)
            {
                Assert.AreEqual(unordered[i], ordered[i]);
            }
        }
    }
}
