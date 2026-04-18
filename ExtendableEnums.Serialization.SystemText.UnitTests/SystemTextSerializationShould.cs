using System.Text.Json;
using ExtendableEnums.Serialization.SystemText.UnitTests.Models;

namespace ExtendableEnums.Serialization.SystemText.UnitTests;

[TestClass]
public class SystemTextSerializationShould
{
    [TestMethod]
    public void DeserializeFromNull()
    {
        var nullSerialized = JsonSerializer.Serialize<SerializableSampleStatus?>(null);
        var status = JsonSerializer.Deserialize<SerializableSampleStatus?>(nullSerialized);
        Assert.IsNull(status);
    }

    [TestMethod]
    public void DeserializeFromObjectGivenNumericValuePropertyNotDeclaredInPrimaryType()
    {
        var status = JsonSerializer.Deserialize<SerializableSampleStatus>($"{{\"value\" : \"{SerializableSampleStatusDeclared.Pending.Value}\"}}");
        Assert.AreEqual(SerializableSampleStatusDeclared.Pending, status);
    }

    [TestMethod]
    public void DeserializeFromObjectWithNoValuePropertyToDefaultValue()
    {
        var status = JsonSerializer.Deserialize<SerializableSampleStatus>($"{{\"id\" : \"{SerializableSampleStatus.Inactive.Value}\"}}");
        Assert.AreEqual(SerializableSampleStatus.Unknown, status);
    }

    [TestMethod]
    public void DeserializeFromObjectWithNumericValueProperty()
    {
        var status = JsonSerializer.Deserialize<SerializableSampleStatus>($"{{\"value\" : {SerializableSampleStatus.Inactive.Value}}}");
        Assert.AreEqual(SerializableSampleStatus.Inactive, status);
    }

    [TestMethod]
    public void DeserializeFromObjectWithStringValueProperty()
    {
        var status = JsonSerializer.Deserialize<SerializableSampleStatusByString>($"{{\"value\" : \"{SerializableSampleStatusByString.Inactive.Value}\"}}");
        Assert.AreEqual(SerializableSampleStatusByString.Inactive, status);
    }

    [TestMethod]
    public void DeserializeFromObjectNotDefined()
    {
        var status = JsonSerializer.Deserialize<SerializableSampleStatus>("{\"value\" : -123}");
        Assert.IsNotNull(status);
        Assert.AreEqual(-123, status.Value);
    }

    [TestMethod]
    public void DeserializeFromSerializedDictionaryGivenExtendedEnumIsKey()
    {
        var dictionary = new SerializableExtendableEnumDictionary<SerializableSampleStatus, string>();
        var keyStatus = SerializableSampleStatus.Discontinued;
        dictionary.Add(keyStatus, "test value");
        var serialized = JsonSerializer.Serialize(dictionary);
        Console.WriteLine(serialized);
        var deserialized = JsonSerializer.Deserialize<SerializableExtendableEnumDictionary<SerializableSampleStatus, string>>(serialized);
        Assert.IsNotNull(deserialized);
        Assert.AreEqual(dictionary[keyStatus], deserialized[keyStatus]);
    }

    [TestMethod]
    public void DeserializeFromTheValueOnly()
    {
        var status = JsonSerializer.Deserialize<SerializableSampleStatus>($"{SerializableSampleStatus.Inactive.Value}");
        Assert.AreEqual(SerializableSampleStatus.Inactive, status);
    }

    [TestMethod]
    public void SerializeTheValueOnly()
    {
        var status = SerializableSampleStatus.Active;
        var serialized = JsonSerializer.Serialize(status);

        Assert.AreEqual($"{status.Value}", serialized);
    }

    [TestMethod]
    public void SerializeToNull()
    {
        SerializableSampleStatus? status = null;
        var serialized = JsonSerializer.Serialize(status);
        Assert.AreEqual("null", serialized);
    }
}