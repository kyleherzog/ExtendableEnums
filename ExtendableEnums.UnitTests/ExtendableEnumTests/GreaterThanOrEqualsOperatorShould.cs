using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests;

[TestClass]
public class GreaterThanOrEqualsOperatorShould
{
    [TestMethod]
    public void ReturnFalseGivenLeftIsNull()
    {
        var right = SampleStatus.Min;
        SampleStatus? left = null;

        Assert.IsFalse(left >= right);
    }

    [TestMethod]
    public void ReturnFalseGivenLeftLessThanRight()
    {
        Assert.IsFalse(SampleStatus.Min >= SampleStatus.Max);
    }

    [TestMethod]
    public void ReturnTrueGivenBothNull()
    {
        SampleStatus? null1 = null;
        SampleStatus? null2 = null;

        Assert.IsTrue(null1 >= null2);
    }

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
    public void ReturnTrueGivenRightIsNull()
    {
        var left = SampleStatus.Min;
        SampleStatus? right = null;

        Assert.IsTrue(left >= right);
    }
}