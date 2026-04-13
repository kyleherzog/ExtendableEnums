using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExtendableEnumTests;

[TestClass]
public class ConstructorShould
{
    [TestMethod]
    public void ThrowArgumentNullExceptionGivenNullValueParmeterPassed()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            Assert.IsNotNull(new NullValueEnum());
        });
    }

    private class NullValueEnum : ExtendableEnumBase<NullValueEnum, string>
    {
        public NullValueEnum()

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type. Justification: Testing that null throws exception.
            : base(null, nameof(NullValueEnum))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        {
        }
    }
}