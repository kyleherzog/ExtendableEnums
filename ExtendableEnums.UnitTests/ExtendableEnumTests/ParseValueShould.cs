using System;
using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests;

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
    [ExpectedException(typeof(ArgumentException))]
    public void ThrowArgumentExceptionGivenNoMatchingValueExists()
    {
        SampleStatus.ParseValue(-1234);
    }
}