using ExtendableEnums.Testing.Models;

namespace ExtendableEnums.UnitTests.ExtendableEnumTests;

[TestClass]
public class ParseValueShould
{
    [TestMethod]
    public void ReturnEnumerationItemGivenMatchingValueExists()
    {
        var result = SampleStatus.ParseValue(SampleStatus.Inactive.Value);
        Assert.AreEqual(SampleStatus.Inactive, result);
    }

    [TestMethod]
    public void ThrowArgumentExceptionGivenNoMatchingValueExists()
    {
        Assert.ThrowsExactly<ArgumentException>(() =>
        {
            SampleStatus.ParseValue(-1234);
        });
    }
}