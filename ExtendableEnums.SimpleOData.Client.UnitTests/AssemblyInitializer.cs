using System.Threading.Tasks;
using ExtendableEnums.OData.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.SimpleOData.Client.UnitTests
{
    [TestClass]
    public class AssemblyInitializer
    {
        public static ITestingHost StaticTestingHost { get; set; }

        [AssemblyCleanup]
        public static void CleanUp()
        {
            StaticTestingHost.Dispose();
        }

        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            context.WriteLine("Initiaizing test assembly...");

            StaticTestingHost = new TestingHost<Startup>("ExtendableEnums.OData.TestHost", deferWebHostCreation: true);
        }
    }
}
