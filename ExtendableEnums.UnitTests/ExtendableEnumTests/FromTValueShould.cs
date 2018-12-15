using ExtendableEnums.UnitTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests
{
    [TestClass]
    public class FromTValueShould
    {
        [TestMethod]
        public void ReturnExpandableEnumerationGivenMatchFound()
        {
            var result = SampleStatus.FromTValue(SampleStatus.Inactive.Value);
            Assert.AreEqual(SampleStatus.Inactive, result);
        }
    }
}
