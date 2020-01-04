using System;
using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExtendableEnumTypeConverterTests
{
    [TestClass]
    public class ConvertFromShould
    {
        [TestMethod]
        public void ReturnTypedValueGivenMatchingDisplayNameMatchFound()
        {
            var converter = new ExtendableEnumTypeConverter(typeof(SampleStatus));
            var status = SampleStatus.Discontinued;
            var result = converter.ConvertFrom(status.DisplayName);
            Assert.AreEqual(status, result);
        }

        [TestMethod]
        public void ReturnTypedValueGivenMatchingStringValueMatchFound()
        {
            var converter = new ExtendableEnumTypeConverter(typeof(SampleStatusByString));
            var status = SampleStatusByString.Discontinued;
            var result = converter.ConvertFrom(status.Value);
            Assert.AreEqual(status, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowArgumentExceptionGivenNoDisplayNameMatchFound()
        {
            var converter = new ExtendableEnumTypeConverter(typeof(SampleStatus));
            converter.ConvertFrom("Can't find this");
            Assert.Fail("An ArgumentException should have been thrown.");
        }

        [TestMethod]
        public void ThrowArgumentExceptionGivenNoDisplayNameOrStringValueMatchFound()
        {
            var value = "unknown-value";
            var converter = new ExtendableEnumTypeConverter(typeof(SampleStatusByString));
            var result = (SampleStatusByString)converter.ConvertFrom(value);
            Assert.AreEqual(value, result.Value);
        }
    }
}
