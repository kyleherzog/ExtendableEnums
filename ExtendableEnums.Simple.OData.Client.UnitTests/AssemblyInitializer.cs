using System.Threading.Tasks;
using ExtendableEnums.OData.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.SimpleOData.Client.UnitTests
{
    [TestClass]
    public class AssemblyInitializer
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

            TestingHost.Instance = new TestingHost(typeof(Startup), "ExtendableEnums.OData.TestHost", deferWebHostCreation: true);
        }
    }
}
