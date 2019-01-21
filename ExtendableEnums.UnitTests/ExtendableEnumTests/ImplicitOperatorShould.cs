using System;
using System.Collections.Generic;
using System.Text;
using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests
{
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
}
