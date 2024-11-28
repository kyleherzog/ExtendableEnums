using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExtendableEnumTests;

[TestClass]
public class TryParseValueShould
{
    [TestMethod]
    public void ReturnFalseGivenNoMatchingValueExists()
    {
        var result = SampleStatus.TryParseValue(-1234, out _);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ReturnTrueAndPopulateResultGivenValueMatchExists()
    {
        var result = SampleStatus.TryParseValue(SampleStatus.Inactive.Value, out var status);
        Assert.IsTrue(result);
        Assert.AreEqual(SampleStatus.Inactive, status);
    }
}