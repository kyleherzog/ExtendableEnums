using System;
using System.Globalization;
using System.Reflection;
using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests
{
    [TestClass]
    public class ParseValueOrCreateShould
    {
        [TestMethod]
        public void ReturnEnumerationItemGivenMatchingValueExists()
        {
            var result = SampleStatus.ParseValueOrCreate(SampleStatus.Inactive.Value);
            Assert.AreEqual(SampleStatus.Inactive, result);
        }

        [TestMethod]
        public void ThrowArgumentExceptionGivenNoMatchingValueExists()
        {
            var value = -123;
            var result = SampleStatus.ParseValueOrCreate(value);
            var constructor = typeof(SampleStatus).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(int), typeof(string), typeof(string) }, null);
            var expected = constructor.Invoke(new object[] { value, value.ToString(CultureInfo.CurrentCulture), null });
            Assert.AreEqual(expected, result);
        }
    }
}
