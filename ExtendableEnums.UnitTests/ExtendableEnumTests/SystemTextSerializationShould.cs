using System.Text.Json;
using ExtendableEnums.Serialization.System.Text.Json;
using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExtendableEnumerationTests;

[TestClass]
public class SystemTextSerializationShould
{
    private JsonSerializerOptions options = new();

    [TestInitialize]
    public void TestInitialize()
    {
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        options.Converters.AddExtendableEnums();
        this.options = options;
    }

    [TestMethod]
    public void DeserializeFromNull()
    {
        var nullSerialized = JsonSerializer.Serialize<SampleStatus?>(null, this.options);
        var status = JsonSerializer.Deserialize<SampleStatus?>(nullSerialized, this.options);
        Assert.IsNull(status);
    }

    [TestMethod]
    public void DeserializeFromObjectGivenNumericValuePripertyNotDeclaredInPrimaryType()
    {
        var status = JsonSerializer.Deserialize<SampleStatus>($"{{\"value\" : \"{SampleStatusDeclared.Pending.Value}\"}}", this.options);
        Assert.AreEqual(SampleStatusDeclared.Pending, status);
    }

    [TestMethod]
    public void DeserializeFromObjectWithNoValuePropertyToDefaultValue()
    {
        var status = JsonSerializer.Deserialize<SampleStatus>($"{{\"id\" : \"{SampleStatus.Inactive.Value}\"}}", this.options);
        Assert.AreEqual(SampleStatus.Unknown, status);
    }

    [TestMethod]
    public void DeserializeFromObjectWithNumericValueProperty()
    {
        var status = JsonSerializer.Deserialize<SampleStatus>($"{{\"value\" : {SampleStatus.Inactive.Value}}}", this.options);
        Assert.AreEqual(SampleStatus.Inactive, status);
    }

    [TestMethod]
    public void DeserializeFromObjectWithStringValueProperty()
    {
        var status = JsonSerializer.Deserialize<SampleStatusByString>($"{{\"value\" : \"{SampleStatusByString.Inactive.Value}\"}}", this.options);
        Assert.AreEqual(SampleStatusByString.Inactive, status);
    }

    [TestMethod]
    public void DeserializeFromObjectNotDefined()
    {
        var status = JsonSerializer.Deserialize<SampleStatus>("{\"value\" : -123}", this.options);
        Assert.AreEqual(-123, status?.Value);
    }

    [TestMethod]
    public void DeserializeFromSerializedDictionaryGivenExtendedEnumIsKey()
    {
        var dictionary = new ExtendableEnumDictionary<SampleStatus, string>();
        var keyStatus = SampleStatus.Discontinued;
        dictionary.Add(keyStatus, "test value");
        var serialized = JsonSerializer.Serialize(dictionary, this.options);
        Console.WriteLine(serialized);
        var deserialized = JsonSerializer.Deserialize<ExtendableEnumDictionary<SampleStatus, string>>(serialized, this.options);
        Assert.AreEqual(dictionary[keyStatus], deserialized?[keyStatus]);
    }

    [TestMethod]
    public void DeserializeFromTheValueOnly()
    {
        var status = JsonSerializer.Deserialize<SampleStatus>($"{SampleStatus.Inactive.Value}", this.options);
        Assert.AreEqual(SampleStatus.Inactive, status);
    }

    [TestMethod]
    public void SerializeTheValueOnly()
    {
        var status = SampleStatus.Active;
        var serialized = JsonSerializer.Serialize(status, this.options);

        Assert.AreEqual($"{status.Value}", serialized);
    }

    [TestMethod]
    public void SerializeToNull()
    {
        SampleStatus? status = null;
        var serialized = JsonSerializer.Serialize(status, this.options);
        Assert.AreEqual("null", serialized);
    }
}