using ExtendableEnums.Testing.Models;

namespace ExtendableEnums.UnitTests.ExtendableEnumTests;

[TestClass]
public class MinShould
{
    [TestMethod]
    public void ReturnTheEnumerationItemWithTheMinimumValue()
    {
        Assert.AreEqual(SampleStatus.Unknown, SampleStatus.Min);
    }
}