using ExtendableEnums.Testing.Models;

namespace ExtendableEnums.UnitTests.ExtendableEnumTests;

[TestClass]
public class MaxShould
{
    [TestMethod]
    public void ReturnTheEnumerationItemWithTheMaximumValue()
    {
        Assert.AreEqual(SampleStatusDeclared.Pending, SampleStatus.Max);
    }
}