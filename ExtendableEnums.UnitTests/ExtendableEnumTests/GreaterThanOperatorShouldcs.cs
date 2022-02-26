using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests;

[TestClass]
public class GreaterThanOperatorShouldcs
{
    [TestMethod]
    public void ReturnFalseGivenLeftEqualsRight()
    {
        var altMax = SampleStatus.ParseValue(SampleStatus.Max.Value);
        Assert.IsFalse(SampleStatus.Max > altMax);
    }

    [TestMethod]
    public void ReturnFalseGivenLeftLessThanRight()
    {
        Assert.IsFalse(SampleStatus.Min > SampleStatus.Max);
    }

    [TestMethod]
    public void ReturnTrueGivenLeftGreaterThanRight()
    {
        Assert.IsTrue(SampleStatus.Max > SampleStatus.Min);
    }
}