using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests
{
    [TestClass]
    public class GetHashCodeShould
    {
        [TestMethod]
        public void ReturnHashCodeOfValue()
        {
            Assert.AreEqual(SampleStatus.Inactive.GetHashCode(), SampleStatus.Inactive.Value.GetHashCode());
        }
    }
}
