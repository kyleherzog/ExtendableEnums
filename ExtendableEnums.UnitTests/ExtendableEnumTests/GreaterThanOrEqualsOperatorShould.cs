using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests;

[TestClass]
public class GreaterThanOrEqualsOperatorShould
{
    [TestMethod]
    public void ReturnTrueGivenLeftEqualsRight()
    {
        var altMax = SampleStatus.ParseValue(SampleStatus.Max.Value);
        Assert.IsTrue(SampleStatus.Max >= altMax);
    }

    [TestMethod]
    public void ReturnTrueGivenLeftGreaterThanRight()
    {
        Assert.IsTrue(SampleStatus.Max >= SampleStatus.Min);
    }

    [TestMethod]
    public void ReturnFalseGivenLeftLessThanRight()
    {
        Assert.IsFalse(SampleStatus.Min >= SampleStatus.Max);
    }
}