using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests;

[TestClass]
public class LessThanOperatorShould
{
    [TestMethod]
    public void ReturnFalseGivenBothNull()
    {
        SampleStatus? null1 = null;
        SampleStatus? null2 = null;

        Assert.IsFalse(null1 < null2);
    }

    [TestMethod]
    public void ReturnFalseGivenLeftEqualsRight()
    {
        var altMax = SampleStatus.ParseValue(SampleStatus.Max.Value);
        Assert.IsFalse(SampleStatus.Max < altMax);
    }

    [TestMethod]
    public void ReturnFalseGivenLeftGreaterThanRight()
    {
        Assert.IsFalse(SampleStatus.Max < SampleStatus.Min);
    }

    [TestMethod]
    public void ReturnFalseGivenRightIsNull()
    {
        var left = SampleStatus.Min;
        SampleStatus? right = null;

        Assert.IsFalse(left < right);
    }

    [TestMethod]
    public void ReturnTrueGivenLeftIsNull()
    {
        var right = SampleStatus.Min;
        SampleStatus? left = null;

        Assert.IsTrue(left < right);
    }

    [TestMethod]
    public void ReturnTrueGivenLeftLessThanRight()
    {
        Assert.IsTrue(SampleStatus.Min < SampleStatus.Max);
    }
}