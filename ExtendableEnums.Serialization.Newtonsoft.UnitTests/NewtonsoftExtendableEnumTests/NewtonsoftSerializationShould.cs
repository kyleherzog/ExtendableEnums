using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ExtendableEnums.Serialization.Newtonsoft.UnitTests.NewtonsoftExtendableEnumTests;

[TestClass]
public class NewtonsoftSerializationShould
{
    [TestMethod]
    public void SerializeToValueGivenNewtonsoftExtendableEnum()
    {
        var status = NewtonsoftSerializableStatus.Active;
        var serialized = JsonConvert.SerializeObject(status);

        Assert.AreEqual($"{status.Value}", serialized);
    }

    [TestMethod]
    public void DeserializeFromValueGivenNewtonsoftExtendableEnum()
    {
        var status = JsonConvert.DeserializeObject<NewtonsoftSerializableStatus>("1");
        Assert.AreEqual(NewtonsoftSerializableStatus.Active, status);
    }

    [TestMethod]
    public void SerializeToNullGivenNullNewtonsoftExtendableEnum()
    {
        NewtonsoftSerializableStatus? status = null;
        var serialized = JsonConvert.SerializeObject(status);
        Assert.AreEqual("null", serialized);
    }

    [TestMethod]
    public void DeserializeFromNullGivenNewtonsoftExtendableEnum()
    {
        var nullSerialized = JsonConvert.SerializeObject((NewtonsoftSerializableStatus?)null);
        var status = JsonConvert.DeserializeObject<NewtonsoftSerializableStatus>(nullSerialized);
        Assert.IsNull(status);
    }

    [TestMethod]
    public void SerializeToValueGivenNewtonsoftExtendableEnumBase()
    {
        var status = NewtonsoftSerializableByString.Alpha;
        var serialized = JsonConvert.SerializeObject(status);

        Assert.AreEqual($"\"{status.Value}\"", serialized);
    }

    [TestMethod]
    public void DeserializeFromValueGivenNewtonsoftExtendableEnumBase()
    {
        var status = JsonConvert.DeserializeObject<NewtonsoftSerializableByString>("\"A\"");
        Assert.AreEqual(NewtonsoftSerializableByString.Alpha, status);
    }

    [TestMethod]
    public void SerializeToNullGivenNullNewtonsoftExtendableEnumBase()
    {
        NewtonsoftSerializableByString? status = null;
        var serialized = JsonConvert.SerializeObject(status);
        Assert.AreEqual("null", serialized);
    }

    private class NewtonsoftSerializableStatus : ExtendableEnum<NewtonsoftSerializableStatus>
    {
        public static readonly NewtonsoftSerializableStatus Active = new(1, nameof(Active));
        public static readonly NewtonsoftSerializableStatus Inactive = new(2, nameof(Inactive));

        private NewtonsoftSerializableStatus(int value, string displayName)
            : base(value, displayName)
        {
        }
    }

    private class NewtonsoftSerializableByString : ExtendableEnumBase<NewtonsoftSerializableByString, string>
    {
        public static readonly NewtonsoftSerializableByString Alpha = new("A", nameof(Alpha));
        public static readonly NewtonsoftSerializableByString Beta = new("B", nameof(Beta));

        private NewtonsoftSerializableByString(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}

