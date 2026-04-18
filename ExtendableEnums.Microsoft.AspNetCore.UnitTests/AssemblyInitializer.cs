using ExtendableEnums.TestHost;
using ExtendableEnums.Testing;

namespace ExtendableEnums.Microsoft.AspNetCore.UnitTests;

[TestClass]
public static class AssemblyInitializer
{
    [AssemblyCleanup]
    public static void CleanUp()
    {
        TestingHost.Instance?.Dispose();
    }

    [AssemblyInitialize]
    public static void Initialize(TestContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        context.WriteLine("Initiaizing test assembly...");

        TestingHost.Instance = new TestingHost(typeof(Startup), "ExtendableEnums.TestHost", deferWebHostCreation: true);
    }
}