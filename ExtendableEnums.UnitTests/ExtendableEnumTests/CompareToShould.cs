using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests;

[TestClass]
public class CompareToShould
{
    [TestMethod]
    public void ReturnPositiveNumberGivenComparedWithNull()
    {
        var status = SampleStatus.Inactive;
        Assert.IsTrue(status.CompareTo(null) > 0);
    }

    [TestMethod]
    public void ReturnSameValueAsComparingWithValue()
    {
        var active = SampleStatus.Active;
        var discontinued = SampleStatus.Discontinued;

        var expectedValue = active.Value.CompareTo(discontinued.Value);
        Assert.AreEqual(expectedValue, active.CompareTo(discontinued));
    }

    [TestMethod]
    public void ReturnZeroGivenSameInstance()
    {
        var active1 = SampleStatus.Active;
        var active2 = SampleStatus.Active;

        Assert.AreEqual(0, active1.CompareTo(active2));
    }
}