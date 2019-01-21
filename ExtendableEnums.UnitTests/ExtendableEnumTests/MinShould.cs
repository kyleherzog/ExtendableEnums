using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests
{
    [TestClass]
    public class MinShould
    {
        [TestMethod]
        public void ReturnTheEnumerationItemWithTheMinimumValue()
        {
            Assert.AreEqual(SampleStatus.Unknown, SampleStatus.Min);
        }
    }
}
