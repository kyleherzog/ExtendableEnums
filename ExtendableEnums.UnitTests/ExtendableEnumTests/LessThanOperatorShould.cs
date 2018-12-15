using ExtendableEnums.UnitTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests
{
    [TestClass]
    public class LessThanOperatorShould
    {
        [TestMethod]
        public void ReturnFalseGivenLeftEqualsRight()
        {
            var altMax = SampleStatus.ParseValue(SampleStatus.Max.Value);
            Assert.IsFalse(SampleStatus.Max < altMax);
        }

        [TestMethod]
        public void ReturnFalseGivenLeftGreaterThanRight()
        {
            Assert.IsFalse(SampleStatus.Max < SampleStatus.Min);
        }

        [TestMethod]
        public void ReturnTrueGivenLeftLessThanRight()
        {
            Assert.IsTrue(SampleStatus.Min < SampleStatus.Max);
        }
    }
}
