using System;
using ExtendableEnums.TestHost;
using ExtendableEnums.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.SimpleOData.Client.UnitTests;

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
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        context.WriteLine("Initiaizing test assembly...");

        TestingHost.Instance = new TestingHost(typeof(Startup), "ExtendableEnums.TestHost", deferWebHostCreation: true);
    }
}