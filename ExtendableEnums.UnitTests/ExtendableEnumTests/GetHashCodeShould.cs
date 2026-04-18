using ExtendableEnums.Testing.Models;

namespace ExtendableEnums.UnitTests.ExtendableEnumTests;

[TestClass]
public class GetHashCodeShould
{
    [TestMethod]
    public void ReturnHashCodeOfValue()
    {
        Assert.AreEqual(SampleStatus.Inactive.GetHashCode(), SampleStatus.Inactive.Value.GetHashCode());
    }
}