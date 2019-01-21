using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests
{
    [TestClass]
    public class LessThanOrEqualsOperatorShould
    {
        [TestMethod]
        public void ReturnFalseGivenLeftGreaterThanRight()
        {
            Assert.IsFalse(SampleStatus.Max <= SampleStatus.Min);
        }

        [TestMethod]
        public void ReturnTrueGivenLeftEqualsRight()
        {
            var altMax = SampleStatus.ParseValue(SampleStatus.Max.Value);
            Assert.IsTrue(SampleStatus.Max <= altMax);
        }

        [TestMethod]
        public void ReturnTrueGivenLeftLessThanRight()
        {
            Assert.IsTrue(SampleStatus.Min <= SampleStatus.Max);
        }
    }
}
