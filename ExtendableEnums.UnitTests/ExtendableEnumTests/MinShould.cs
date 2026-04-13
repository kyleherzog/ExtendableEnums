using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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