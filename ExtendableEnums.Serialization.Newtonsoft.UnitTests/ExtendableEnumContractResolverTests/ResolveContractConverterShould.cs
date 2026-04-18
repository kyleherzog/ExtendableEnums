using FluentAssertions;

namespace ExtendableEnums.Serialization.Newtonsoft.UnitTests.ExtendableEnumContractResolverTests;

[TestClass]
public class ResolveContractConverterShould
{
    [TestMethod]
    public void ReturnExtendableEnumJsonConverterGivenExtendableEnumType()
    {
        var resolver = new ExtendableEnumContractResolver();
        var method = typeof(ExtendableEnumContractResolver).GetMethod("ResolveContractConverter", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var result = method?.Invoke(resolver, [typeof(Testing.Models.SampleStatus)]);

        result.Should().NotBeNull();
        result.Should().BeOfType<ExtendableEnumJsonConverter>();
    }

    [TestMethod]
    public void ReturnExtendableEnumDictionaryJsonConverterGivenExtendableEnumDictionaryType()
    {
        var resolver = new ExtendableEnumContractResolver();
        var method = typeof(ExtendableEnumContractResolver).GetMethod("ResolveContractConverter", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var result = method?.Invoke(resolver, [typeof(SerializableExtendableEnumDictionary<Testing.Models.SampleStatus, string>)]);

        result.Should().NotBeNull();
        result.Should().BeOfType<ExtendableEnumDictionaryJsonConverter>();
    }

    [TestMethod]
    public void ReturnNullGivenNonExtendableEnumType()
    {
        var resolver = new ExtendableEnumContractResolver();
        var method = typeof(ExtendableEnumContractResolver).GetMethod("ResolveContractConverter", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var result = method?.Invoke(resolver, [typeof(string)]);

        result.Should().BeNull();
    }
}