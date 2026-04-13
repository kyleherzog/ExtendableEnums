using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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