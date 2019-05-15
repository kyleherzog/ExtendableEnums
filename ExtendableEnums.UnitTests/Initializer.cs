﻿using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.UnitTests
{
    [TestClass]
    public static class Initializer
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            context.WriteLine("Initializing test assembly...");
            SampleStatus.DeclaringTypes.Add(typeof(SampleStatusDeclared));
        }
    }
}
