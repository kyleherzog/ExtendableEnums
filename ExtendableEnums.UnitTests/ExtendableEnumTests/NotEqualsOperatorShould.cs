using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests;

[TestClass]
public class NotEqualsOperatorShould
{
    [TestMethod]
    public void ReturnFalseGivenMatchingInstances()
    {
        var active1 = SampleStatus.Active;
        var active2 = SampleStatus.Active;

        Assert.IsFalse(active1 != active2);
    }

    [TestMethod]
    public void ReturnFalseGivenMatchingValues()
    {
        var deleted = SampleStatus.Deleted;
        var discontinued = SampleStatus.Discontinued;

        Assert.IsFalse(deleted != discontinued);
    }

    [TestMethod]
    public void ReturnTrueGivenDifferentValues()
    {
        var active = SampleStatus.Active;
        var inactive = SampleStatus.Inactive;

        Assert.IsTrue(active != inactive);
    }
}