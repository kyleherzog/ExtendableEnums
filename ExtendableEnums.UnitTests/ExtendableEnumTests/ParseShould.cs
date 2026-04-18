using ExtendableEnums.Testing.Models;

namespace ExtendableEnums.UnitTests.ExtendableEnumTests;

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
    public void ThrowArgumentExceptionGivenNoMatchingDisplayNameExists()
    {
        Assert.ThrowsExactly<ArgumentException>(() =>
        {
            SampleStatus.Parse(Guid.NewGuid().ToString());
        });
    }
}