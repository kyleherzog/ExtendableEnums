using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests
{
    [TestClass]
    public class TryParseShould
    {
        [TestMethod]
        public void ReturnFalseGivenDisplayNameMatchDoesNotExist()
        {
            var result = SampleStatus.TryParse(System.Guid.NewGuid().ToString(), out SampleStatus status);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ReturnTrueAndPopulateResultGivenDisplayNameMatchExists()
        {
            var result = SampleStatus.TryParse("Inactive", out SampleStatus status);
            Assert.IsTrue(result);
            Assert.AreEqual(SampleStatus.Inactive, status);
        }
    }
}
