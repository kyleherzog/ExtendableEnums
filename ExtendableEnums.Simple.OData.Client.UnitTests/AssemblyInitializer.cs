using ExtendableEnums.TestHost;
using ExtendableEnums.Testing;

namespace ExtendableEnums.Simple.OData.Client.UnitTests;

[TestClass]
public static class AssemblyInitializer
{
    [AssemblyCleanup]
    public static void CleanUp()
    {
        TestingHost.GetRequiredInstance().Dispose();
    }

    [AssemblyInitialize]
    public static void Initialize(TestContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        context.WriteLine("Initializing test assembly...");

        TestingHost.Instance = new TestingHost(typeof(Startup), "ExtendableEnums.TestHost", deferWebHostCreation: true);
    }
}