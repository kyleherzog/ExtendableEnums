using ExtendableEnums.Testing.Models;

namespace ExtendableEnums.Simple.OData.Client.UnitTests.ExtendableEnumConverterTests;

[TestClass]
public class ConvertShould
{
    [TestMethod]
    public void ConvertValueOnlyToSampleStatusGivenTypesPassed()
    {
        var dictionary = new Dictionary<string, object>
        {
            { "value", 1 },
        };

        var result = ExtendableEnumConverter.Convert(typeof(SampleStatus), typeof(int), dictionary);
        Assert.AreEqual(SampleStatus.Active, result);
    }
}