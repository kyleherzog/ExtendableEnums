using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.TypeExtensionsTests
{
    [TestClass]
    public class IsExtendableEnumShould
    {
        [TestMethod]
        public void ReturnTrueGivenTypeDerivesFromExtendableEnum()
        {
            var result = typeof(SampleStatus).IsExtendableEnum();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReturnTrueGivenTypeDerivesFromExtendableEnumBase()
        {
            var result = typeof(SampleStatusByString).IsExtendableEnum();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReturnFalseGivenTypeDoesNotDeriveFromExtendableEnumBase()
        {
            var result = typeof(string).IsExtendableEnum();
            Assert.IsFalse(result);
        }
    }
}
