using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExtendableEnumTests;

[TestClass]
public class TryParseShould
{
    [TestMethod]
    public void ReturnFalseGivenDisplayNameMatchDoesNotExist()
    {
        var result = SampleStatus.TryParse(Guid.NewGuid().ToString(), out _);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ReturnTrueAndPopulateResultGivenDisplayNameMatchExists()
    {
        var result = SampleStatus.TryParse("Inactive", out var status);
        Assert.IsTrue(result);
        Assert.AreEqual(SampleStatus.Inactive, status);
    }
}