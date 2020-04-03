using ExtendableEnums.Testing.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ExtendableEnums.UnitTests.ExtendableEnumDictionaryTests
{
    [TestClass]
    public class DeserializeShould
    {
        [TestMethod]
        public void DeserializeGivenValidSerialized()
        {
            var serialized = "{ \"1\":\"Active\",\"2\":\"Deleted\"}";

            var expected = new ExtendableEnumDictionary<SampleStatus, string>
            {
                { SampleStatus.Active, nameof(SampleStatus.Active) },
                { SampleStatus.Deleted, nameof(SampleStatus.Deleted) },
            };

            var result = JsonConvert.DeserializeObject<ExtendableEnumDictionary<SampleStatus, string>>(serialized);

            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DeserializeGivenDisplayNameSerialized()
        {
            var serialized = "{ \"Active\":\"Active\",\"Deleted\":\"Deleted\"}";

            var expected = new ExtendableEnumDictionary<SampleStatus, string>
            {
                { SampleStatus.Active, nameof(SampleStatus.Active) },
                { SampleStatus.Deleted, nameof(SampleStatus.Deleted) },
            };

            var result = JsonConvert.DeserializeObject<ExtendableEnumDictionary<SampleStatus, string>>(serialized);

            result.Should().BeEquivalentTo(expected);
        }
    }
}
