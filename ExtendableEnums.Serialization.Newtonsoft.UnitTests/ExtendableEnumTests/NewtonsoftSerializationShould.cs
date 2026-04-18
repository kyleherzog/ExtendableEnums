using ExtendableEnums.Serialization.Newtonsoft.UnitTests.Models;
using Newtonsoft.Json;

namespace ExtendableEnums.Serialization.Newtonsoft.UnitTests.ExtendableEnumTests;

[TestClass]
public class NewtonsoftSerializationShould
{
    [TestMethod]
    public void DeserializeFromNull()
    {
        var nullSerialized = JsonConvert.SerializeObject((SerializableSampleStatus?)null);
        var status = JsonConvert.DeserializeObject<SerializableSampleStatus>(nullSerialized);
        Assert.IsNull(status);
    }

    [TestMethod]
    public void DeserializeFromObjectGivenNumericValuePropertyNotDeclaredInPrimaryType()
    {
        var status = JsonConvert.DeserializeObject<SerializableSampleStatus>($"{{\"value\" : \"{SerializableSampleStatusDeclared.Pending.Value}\"}}");
        Assert.AreEqual(SerializableSampleStatusDeclared.Pending, status);
    }

    [TestMethod]
    public void DeserializeFromObjectWithNoValuePropertyToDefaultValue()
    {
        var status = JsonConvert.DeserializeObject<SerializableSampleStatus>($"{{\"id\" : \"{SerializableSampleStatus.Inactive.Value}\"}}");
        Assert.AreEqual(SerializableSampleStatus.Unknown, status);
    }

    [TestMethod]
    public void DeserializeFromObjectWithNumericValueProperty()
    {
        var status = JsonConvert.DeserializeObject<SerializableSampleStatus>($"{{\"value\" : {SerializableSampleStatus.Inactive.Value}}}");
        Assert.AreEqual(SerializableSampleStatus.Inactive, status);
    }

    [TestMethod]
    public void DeserializeFromObjectWithStringValueProperty()
    {
        var status = JsonConvert.DeserializeObject<SerializableSampleStatusByString>($"{{\"value\" : \"{SerializableSampleStatusByString.Inactive.Value}\"}}");
        Assert.AreEqual(SerializableSampleStatusByString.Inactive, status);
    }

    [TestMethod]
    public void DeserializeFromObjectNotDefined()
    {
        var status = JsonConvert.DeserializeObject<SerializableSampleStatus>("{\"value\" : -123}");
        Assert.AreEqual(-123, status?.Value);
    }

    [TestMethod]
    public void DeserializeFromSerializedDictionaryGivenExtendedEnumIsKey()
    {
        var dictionary = new SerializableExtendableEnumDictionary<SerializableSampleStatus, string>();
        var keyStatus = SerializableSampleStatus.Discontinued;
        dictionary.Add(keyStatus, "test value");
        var serialized = JsonConvert.SerializeObject(dictionary);
        Console.WriteLine(serialized);
        var deserialized = JsonConvert.DeserializeObject<SerializableExtendableEnumDictionary<SerializableSampleStatus, string>>(serialized);
        Assert.AreEqual(dictionary[keyStatus], deserialized?[keyStatus]);
    }

    [TestMethod]
    public void DeserializeFromTheValueOnly()
    {
        var status = JsonConvert.DeserializeObject<SerializableSampleStatus>($"{SerializableSampleStatus.Inactive.Value}");
        Assert.AreEqual(SerializableSampleStatus.Inactive, status);
    }

    [TestMethod]
    public void SerializeTheValueOnly()
    {
        var status = SerializableSampleStatus.Active;
        var serialized = JsonConvert.SerializeObject(status);

        Assert.AreEqual($"{status.Value}", serialized);
    }

    [TestMethod]
    public void SerializeToNull()
    {
        SerializableSampleStatus? status = null;
        var serialized = JsonConvert.SerializeObject(status);
        Assert.AreEqual("null", serialized);
    }
}