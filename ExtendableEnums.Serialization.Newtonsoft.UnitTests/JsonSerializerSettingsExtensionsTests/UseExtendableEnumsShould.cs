using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ExtendableEnums.Serialization.Newtonsoft.UnitTests.JsonSerializerSettingsExtensionsTests;

[TestClass]
public class UseExtendableEnumsShould
{
    [TestMethod]
    public void ReturnSameSettingsInstance()
    {
        var settings = new JsonSerializerSettings();

        var result = settings.UseExtendableEnums();

        result.Should().BeSameAs(settings);
    }

    [TestMethod]
    public void SetContractResolverToExtendableEnumContractResolver()
    {
        var settings = new JsonSerializerSettings();

        settings.UseExtendableEnums();

        settings.ContractResolver.Should().NotBeNull();
        settings.ContractResolver.Should().BeOfType<ExtendableEnumContractResolver>();
    }

    [TestMethod]
    public void ThrowArgumentNullExceptionGivenNullSettings()
    {
        JsonSerializerSettings? settings = null;

        var act = () => settings!.UseExtendableEnums();

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("settings");
    }

    [TestMethod]
    public void SerializeExtendableEnumWithoutExplicitConverterAttribute()
    {
        var settings = new JsonSerializerSettings().UseExtendableEnums();
        var status = UnattributedStatus.Active;

        var serialized = JsonConvert.SerializeObject(status, settings);

        serialized.Should().Be($"{status.Value}");
    }

    [TestMethod]
    public void DeserializeExtendableEnumWithoutExplicitConverterAttribute()
    {
        var settings = new JsonSerializerSettings().UseExtendableEnums();
        var expectedStatus = UnattributedStatus.Active;

        var deserialized = JsonConvert.DeserializeObject<UnattributedStatus>($"{expectedStatus.Value}", settings);

        deserialized.Should().NotBeNull();
        deserialized.Should().Be(expectedStatus);
    }

    private class UnattributedStatus : ExtendableEnum<UnattributedStatus>
    {
        public static readonly UnattributedStatus Active = new(1, nameof(Active));
        public static readonly UnattributedStatus Inactive = new(2, nameof(Inactive));

        private UnattributedStatus(int value, string displayName)
            : base(value, displayName)
        {
        }
    }
}