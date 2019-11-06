using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests
{
    [TestClass]
    public class ToStringShould
    {
        [TestMethod]
        public void ReturnDisplayName()
        {
            var status = SampleStatus.Active;
            Assert.AreEqual(status.DisplayName, status.ToString());
        }
    }
}
