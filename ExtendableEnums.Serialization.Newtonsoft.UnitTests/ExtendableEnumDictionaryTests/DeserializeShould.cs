using ExtendableEnums.Serialization.Newtonsoft.UnitTests.Models;
using FluentAssertions;
using Newtonsoft.Json;

namespace ExtendableEnums.Serialization.Newtonsoft.UnitTests.ExtendableEnumDictionaryTests;

[TestClass]
public class DeserializeShould
{
    [TestMethod]
    public void DeserializeGivenValidSerialized()
    {
        var serialized = "{ \"1\":\"Active\",\"2\":\"Deleted\"}";

        var expected = new SerializableExtendableEnumDictionary<SerializableSampleStatus, string>
        {
            { SerializableSampleStatus.Active, nameof(SerializableSampleStatus.Active) },
            { SerializableSampleStatus.Deleted, nameof(SerializableSampleStatus.Deleted) },
        };

        var result = JsonConvert.DeserializeObject<SerializableExtendableEnumDictionary<SerializableSampleStatus, string>>(serialized);

        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void DeserializeGivenValidSerializedByString()
    {
        var serialized = "{ \"B\":\"Active\",\"C\":\"Deleted\"}";

        var expected = new SerializableExtendableEnumDictionary<SerializableSampleStatusByString, string>
        {
            { SerializableSampleStatusByString.Active, nameof(SerializableSampleStatusByString.Active) },
            { SerializableSampleStatusByString.Deleted, nameof(SerializableSampleStatusByString.Deleted) },
        };

        var result = JsonConvert.DeserializeObject<SerializableExtendableEnumDictionary<SerializableSampleStatusByString, string>>(serialized);

        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void DeserializeGivenDisplayNameSerializedByString()
    {
        Console.WriteLine(JsonConvert.SerializeObject(SerializableSampleStatusByString.Active));
        var serialized = "{ \"Active\":\"Active\",\"Deleted\":\"Deleted\"}";

        var expected = new SerializableExtendableEnumDictionary<SerializableSampleStatusByString, string>
        {
            { SerializableSampleStatusByString.Active, nameof(SerializableSampleStatusByString.Active) },
            { SerializableSampleStatusByString.Deleted, nameof(SerializableSampleStatusByString.Deleted) },
        };

        var result = JsonConvert.DeserializeObject<SerializableExtendableEnumDictionary<SerializableSampleStatusByString, string>>(serialized);

        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void DeserializeGivenDisplayNameSerialized()
    {
        Console.WriteLine(JsonConvert.SerializeObject(SerializableSampleStatus.Active));
        var serialized = "{ \"Active\":\"Active\",\"Deleted\":\"Deleted\"}";

        var expected = new SerializableExtendableEnumDictionary<SerializableSampleStatus, string>
        {
            { SerializableSampleStatus.Active, nameof(SerializableSampleStatus.Active) },
            { SerializableSampleStatus.Deleted, nameof(SerializableSampleStatus.Deleted) },
        };

        var result = JsonConvert.DeserializeObject<SerializableExtendableEnumDictionary<SerializableSampleStatus, string>>(serialized);

        result.Should().BeEquivalentTo(expected);
    }
}