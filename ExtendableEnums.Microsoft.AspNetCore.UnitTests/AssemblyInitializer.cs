using ExtendableEnums.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.Microsoft.AspNetCore.UnitTests
{
    [TestClass]
    public static class AssemblyInitializer
    {
        [AssemblyCleanup]
        public static void CleanUp()
        {
            TestingHost.Instance.Dispose();
        }

        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            context.WriteLine("Initiaizing test assembly...");

            TestingHost.Instance = new TestingHost(typeof(Startup), "ExtendableEnums.TestHost", deferWebHostCreation: true);
        }
    }
}
