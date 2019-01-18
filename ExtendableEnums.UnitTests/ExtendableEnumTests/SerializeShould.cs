using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests
{
    [TestClass]
    public class SerializeShould
    {
        [TestMethod]
        public void DeserializeFromTheValueOnly()
        {
            var status = JsonConvert.DeserializeObject<SampleStatus>($"{SampleStatus.Inactive.Value}");
            Assert.AreEqual(SampleStatus.Inactive, status);
        }

        [TestMethod]
        public void SerializeTheValueOnly()
        {
            var status = SampleStatus.Active;
            var serialized = JsonConvert.SerializeObject(status);

            Assert.AreEqual($"{status.Value}", serialized);
        }
    }
}
