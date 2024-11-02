using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ExtendableEnums.UnitTests.ExtendableEnumDictionaryTests;

[TestClass]
public class SerializeShould
{
    [TestMethod]
    public void SerializeKeyAsValueOnlyGivenIntValue()
    {
        var items = new ExtendableEnumDictionary<SampleStatus, string>
        {
            { SampleStatus.Active, nameof(SampleStatus.Active) },
            { SampleStatus.Deleted, nameof(SampleStatus.Deleted) },
        };

        var serialized = JsonConvert.SerializeObject(items);
        Assert.AreEqual("{\"1\":\"Active\",\"2\":\"Deleted\"}", serialized);
    }

    [TestMethod]
    public void SerializeKeyAsValueOnlyGivenStringValue()
    {
        var items = new ExtendableEnumDictionary<SampleStatusByString, string>
        {
            { SampleStatusByString.Active, nameof(SampleStatusByString.Active) },
            { SampleStatusByString.Deleted, nameof(SampleStatusByString.Deleted) },
        };

        var serialized = JsonConvert.SerializeObject(items);
        Assert.AreEqual("{\"B\":\"Active\",\"C\":\"Deleted\"}", serialized);
    }
}