using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests;

[TestClass]
public class EqualsOperatorShould
{
    [TestMethod]
    public void ReturnFalseGivenDifferentValues()
    {
        var active = SampleStatus.Active;
        var inactive = SampleStatus.Inactive;

        Assert.IsFalse(active == inactive);
    }

    [TestMethod]
    public void ReturnTrueGivenMatchingInstances()
    {
        var active1 = SampleStatus.Active;
        var active2 = SampleStatus.Active;

        Assert.IsTrue(active1 == active2);
    }

    [TestMethod]
    public void ReturnTrueGivenMatchingValues()
    {
        var deleted = SampleStatus.Deleted;
        var discontinued = SampleStatus.Discontinued;

        Assert.IsTrue(deleted == discontinued);
    }
}