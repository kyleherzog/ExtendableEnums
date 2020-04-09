using System;
using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests
{
    [TestClass]
    public class ComparableCompareToObjectShould
    {
        [TestMethod]
        public void ReturnPositiveNumberGivenComparedWithNull()
        {
            IComparable status = SampleStatus.Inactive;
            Assert.IsTrue(status.CompareTo(null) > 0);
        }

        [TestMethod]
        public void ReturnSameValueAsComparingWithValue()
        {
            IComparable active = SampleStatus.Active;
            IComparable discontinued = SampleStatus.Discontinued;

            var expectedValue = ((SampleStatus)active).Value.CompareTo(((SampleStatus)discontinued).Value);
            Assert.AreEqual(expectedValue, active.CompareTo(discontinued));
        }

        [TestMethod]
        public void ReturnSameValueAsComparingWithValueGiveValueTypePassed()
        {
            IComparable active = SampleStatus.Active;
            var discontinued = SampleStatus.Discontinued.Value;

            var expectedValue = ((SampleStatus)active).Value.CompareTo(discontinued);
            Assert.AreEqual(expectedValue, active.CompareTo(discontinued));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowExceptionGivenComparingAgainstNoncomparableValueType()
        {
            IComparable active = SampleStatus.Active;

            active.CompareTo("BAD-OBJECT");
            Assert.Fail();
        }

        [TestMethod]
        public void ReturnZeroGivenSameInstance()
        {
            IComparable active1 = SampleStatus.Active;
            IComparable active2 = SampleStatus.Active;

            Assert.AreEqual(0, active1.CompareTo(active2));
        }
    }
}
