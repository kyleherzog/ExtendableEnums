using ExtendableEnums.UnitTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests
{
    [TestClass]
    public class MinShould
    {
        [TestMethod]
        public void ReturnTheEnumerationItemWithTheMinimumValue()
        {
            Assert.AreEqual(SampleStatus.Active, SampleStatus.Min);
        }
    }
}
