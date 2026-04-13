using ExtendableEnums.SimpleOData.Client;
using ExtendableEnums.TestHost;
using ExtendableEnums.Testing;
using ExtendableEnums.Testing.Models;
using Simple.OData.Client;

namespace ExtendableEnums.Simple.OData.Client.UnitTests.ExtendableEnumConverterTests;

[TestClass]
public class RegisterShould
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    public async Task RegisterConverterGivenGenericMethodCalled()
    {
        await TestingHost.GetRequiredInstance().GetNewWebHost().ConfigureAwait(true);
        var settings = new ODataClientSettings
        {
            BaseUri = TestingHost.GetRequiredInstance().BaseODataUrl,
        };

        settings.RegisterExtendableEnum<SampleStatus>();
        var client = new ODataClient(settings);

        var target = DataContext.Books[0];

        var book = await client
            .For<SampleBook>()
            .Key(target.Id)
            .FindEntryAsync(TestContext.CancellationToken)
            .ConfigureAwait(true);

        Assert.AreEqual(target.Status, book.Status);
    }

    [TestMethod]
    public async Task RegisterConverterGivenGenericMethodCalledAndPosted()
    {
        await TestingHost.GetRequiredInstance().GetNewWebHost().ConfigureAwait(true);
        var settings = new ODataClientSettings
        {
            BaseUri = TestingHost.GetRequiredInstance().BaseODataUrl,
            IgnoreUnmappedProperties = true,
        };

        settings.RegisterExtendableEnum<SampleStatus>();
        var client = new ODataClient(settings);

        var originalCount = DataContext.Books.Count;

        var novel = new SampleBook
        {
            Id = Guid.NewGuid().ToString(),
            Title = "The Never Ending Novel",
            Status = SampleStatus.Active,
        };

        await client
            .For<SampleBook>()
            .Set(novel)
            .InsertEntryAsync(TestContext.CancellationToken)
            .ConfigureAwait(true);

        Assert.HasCount(originalCount + 1, DataContext.Books);
    }

    [TestMethod]
    public async Task RegisterConverterGivenNonGenericMethodCalled()
    {
        await TestingHost.GetRequiredInstance().GetNewWebHost().ConfigureAwait(true);
        var settings = new ODataClientSettings
        {
            BaseUri = TestingHost.GetRequiredInstance().BaseODataUrl,
        };
        settings.RegisterExtendableEnum(typeof(SampleStatus));
        var client = new ODataClient(settings);

        var target = DataContext.Books[0];

        var book = await client
            .For<SampleBook>()
            .Key(target.Id)
            .FindEntryAsync(TestContext.CancellationToken)
            .ConfigureAwait(true);

        Assert.AreEqual(target.Status, book.Status);
    }

    [TestMethod]
    public async Task ThrowArgumentExceptionGivenTypeIsNotExtendableEnumDescendant()
    {
        await TestingHost.GetRequiredInstance().GetNewWebHost().ConfigureAwait(true);
        var settings = new ODataClientSettings
        {
            BaseUri = TestingHost.GetRequiredInstance().BaseODataUrl,
        };

        Assert.ThrowsExactly<ArgumentException>(() => settings.RegisterExtendableEnum(typeof(string)));
    }
}