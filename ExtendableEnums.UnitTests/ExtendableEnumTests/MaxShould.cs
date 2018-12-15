using ExtendableEnums.UnitTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests
{
    [TestClass]
    public class MaxShould
    {
        [TestMethod]
        public void ReturnTheEnumerationItemWithTheMaximumValue()
        {
            Assert.AreEqual(SampleStatus.Inactive, SampleStatus.Max);
        }
    }
}
