using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests;

[TestClass]
public class ParseShould
{
    [TestMethod]
    public void ReturnEnumerationItemGivenMatchingDisplayNameExists()
    {
        var result = SampleStatus.Parse("Inactive");
        Assert.AreEqual(SampleStatus.Inactive, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ThrowArgumentExceptionGivenNoMatchingDisplayNameExists()
    {
        SampleStatus.Parse(Guid.NewGuid().ToString());
    }
}