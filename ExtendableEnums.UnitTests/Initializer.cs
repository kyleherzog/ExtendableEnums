using ExtendableEnums.Testing.Models;

namespace ExtendableEnums.UnitTests;

[TestClass]
public static class Initializer
{
    [AssemblyInitialize]
    public static void Initialize(TestContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        context.WriteLine("Initializing test assembly...");
        SampleStatus.DeclaringTypes.Add(typeof(SampleStatusDeclared));
    }
}