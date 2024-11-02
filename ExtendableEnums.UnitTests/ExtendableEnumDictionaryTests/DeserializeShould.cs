using ExtendableEnums.Testing.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ExtendableEnums.UnitTests.ExtendableEnumDictionaryTests;

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
    public void DeserializeGivenValidSerializedByString()
    {
        var serialized = "{ \"B\":\"Active\",\"C\":\"Deleted\"}";

        var expected = new ExtendableEnumDictionary<SampleStatusByString, string>
        {
            { SampleStatusByString.Active, nameof(SampleStatusByString.Active) },
            { SampleStatusByString.Deleted, nameof(SampleStatusByString.Deleted) },
        };

        var result = JsonConvert.DeserializeObject<ExtendableEnumDictionary<SampleStatusByString, string>>(serialized);

        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void DeserializeGivenDisplayNameSerializedByString()
    {
        Console.WriteLine(JsonConvert.SerializeObject(SampleStatusByString.Active));
        var serialized = "{ \"Active\":\"Active\",\"Deleted\":\"Deleted\"}";

        var expected = new ExtendableEnumDictionary<SampleStatusByString, string>
        {
            { SampleStatusByString.Active, nameof(SampleStatusByString.Active) },
            { SampleStatusByString.Deleted, nameof(SampleStatusByString.Deleted) },
        };

        var result = JsonConvert.DeserializeObject<ExtendableEnumDictionary<SampleStatusByString, string>>(serialized);

        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void DeserializeGivenDisplayNameSerialized()
    {
        Console.WriteLine(JsonConvert.SerializeObject(SampleStatus.Active));
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