using ExtendableEnums.Testing.Models;

namespace ExtendableEnums.UnitTests.ExtendableEnumTests;

[TestClass]
public class FromTValueShould
{
    [TestMethod]
    public void ReturnExpandableEnumerationGivenMatchFound()
    {
        var result = SampleStatus.FromTValue(SampleStatus.Inactive.Value);
        Assert.AreEqual(SampleStatus.Inactive, result);
    }
}