using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests;

[TestClass]
public class LessThanOrEqualsOperatorShould
{
    [TestMethod]
    public void ReturnFalseGivenLeftGreaterThanRight()
    {
        Assert.IsFalse(SampleStatus.Max <= SampleStatus.Min);
    }

    [TestMethod]
    public void ReturnFalseGivenRightIsNull()
    {
        var left = SampleStatus.Min;
        SampleStatus? right = null;

        Assert.IsFalse(left <= right);
    }

    [TestMethod]
    public void ReturnTrueGivenBothNull()
    {
        SampleStatus? null1 = null;
        SampleStatus? null2 = null;

        Assert.IsTrue(null1 <= null2);
    }

    [TestMethod]
    public void ReturnTrueGivenLeftEqualsRight()
    {
        var altMax = SampleStatus.ParseValue(SampleStatus.Max.Value);
        Assert.IsTrue(SampleStatus.Max <= altMax);
    }

    [TestMethod]
    public void ReturnTrueGivenLeftIsNull()
    {
        var right = SampleStatus.Min;
        SampleStatus? left = null;

        Assert.IsTrue(left <= right);
    }

    [TestMethod]
    public void ReturnTrueGivenLeftLessThanRight()
    {
        Assert.IsTrue(SampleStatus.Min <= SampleStatus.Max);
    }
}