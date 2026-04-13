using ExtendableEnums.Serialization.Newtonsoft.UnitTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ExtendableEnums.Serialization.Newtonsoft.UnitTests.ExtendableEnumDictionaryTests;

[TestClass]
public class SerializeShould
{
    [TestMethod]
    public void SerializeKeyAsValueOnlyGivenIntValue()
    {
        var items = new SerializableExtendableEnumDictionary<SerializableSampleStatus, string>
        {
            { SerializableSampleStatus.Active, nameof(SerializableSampleStatus.Active) },
            { SerializableSampleStatus.Deleted, nameof(SerializableSampleStatus.Deleted) },
        };

        var serialized = JsonConvert.SerializeObject(items);
        Assert.AreEqual("{\"1\":\"Active\",\"2\":\"Deleted\"}", serialized);
    }

    [TestMethod]
    public void SerializeKeyAsValueOnlyGivenStringValue()
    {
        var items = new SerializableExtendableEnumDictionary<SerializableSampleStatusByString, string>
        {
            { SerializableSampleStatusByString.Active, nameof(SerializableSampleStatusByString.Active) },
            { SerializableSampleStatusByString.Deleted, nameof(SerializableSampleStatusByString.Deleted) },
        };

        var serialized = JsonConvert.SerializeObject(items);
        Assert.AreEqual("{\"B\":\"Active\",\"C\":\"Deleted\"}", serialized);
    }
}