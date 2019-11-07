using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests
{
    [TestClass]
    public class ConstructorShould
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowArgumentNullExceptionGivenNullValueParmeterPassed()
        {
            Assert.IsNotNull(new NullValueEnum());
        }

        private class NullValueEnum : ExtendableEnumBase<NullValueEnum, string>
        {
            public NullValueEnum()
                : base(null, nameof(NullValueEnum))
            {
            }
        }
    }
}
