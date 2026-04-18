using ExtendableEnums.Core;
using ExtendableEnums.Testing.Models;

namespace ExtendableEnums.UnitTests.ExtendableEnumTests;

[TestClass]
public class ImplicitOperatorShould
{
    [TestMethod]
    public void ConvertToExpandedEnumerationFromValue()
    {
        ExtendableEnumBase<SampleStatus, int> status = SampleStatus.Active.Value;
        Assert.AreEqual(SampleStatus.Active, status);
    }
}