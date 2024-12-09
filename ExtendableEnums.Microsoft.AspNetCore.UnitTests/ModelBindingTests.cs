using System.Net;
using ExtendableEnums.Testing;
using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ExtendableEnums.Microsoft.AspNetCore.UnitTests;

[TestClass]
public class ModelBindingTests : IDisposable
{
    private readonly HttpClient client = new();

    private bool hasDisposed;

    private JsonSerializerSettings options = new();

    [TestInitialize]
    public void TestInitialize()
    {
        var options = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        };
        options.Converters.AddExtendableEnums();
        this.options = options;
    }

    ~ModelBindingTests()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing).
        Dispose(false);
    }

    [TestMethod]
    public async Task BindTheExtendableEnumCorrectlyGivenIntBasedValue()
    {
        await TestingHost.GetRequiredInstance().GetNewWebHost().ConfigureAwait(true);

        var values = new Dictionary<string, string>()
        {
            { "id", "4" },
            { "title", "My Title" },
            { "status", "2" },
        };

        using var content = new FormUrlEncodedContent(values);
        var targetUrl = new Uri($"{TestingHost.GetRequiredInstance().Address}/samplebooks/edit/1");
        using var response = await client.PostAsync(targetUrl, content).ConfigureAwait(true);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
        var book = JsonConvert.DeserializeObject<SampleBook>(responseContent, this.options);

        Assert.AreEqual(SampleStatus.Deleted, book?.Status);
    }

    [TestMethod]
    public async Task BindTheExtendableEnumCorrectlyGivenNonExistentIntBasedValue()
    {
        await TestingHost.GetRequiredInstance().GetNewWebHost().ConfigureAwait(true);

        var values = new Dictionary<string, string>()
        {
            { "id", "4" },
            { "title", "My Title" },
        };

        using var content = new FormUrlEncodedContent(values);
        var targetUrl = new Uri($"{TestingHost.GetRequiredInstance().Address}/samplebooks/edit/1");
        using var response = await client.PostAsync(targetUrl, content).ConfigureAwait(true);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
        var book = JsonConvert.DeserializeObject<SampleBook>(responseContent, this.options);

        Assert.IsNull(book?.Status);
    }

    [TestMethod]
    public async Task BindTheExtendableEnumCorrectlyGivenNullIntBasedValue()
    {
        await TestingHost.GetRequiredInstance().GetNewWebHost().ConfigureAwait(true);

        var values = new Dictionary<string, string?>()
        {
            { "id", "4" },
            { "title", "My Title" },
            { "status", null },
        };

        using var content = new FormUrlEncodedContent(values);
        var targetUrl = new Uri($"{TestingHost.GetRequiredInstance().Address}/samplebooks/edit/1");
        using var response = await client.PostAsync(targetUrl, content).ConfigureAwait(true);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
        var book = JsonConvert.DeserializeObject<SampleBook>(responseContent, this.options);

        Assert.IsNull(book?.Status);
    }

    [TestMethod]
    public async Task BindTheExtendableEnumCorrectlyGivenStringBasedValue()
    {
        await TestingHost.GetRequiredInstance().GetNewWebHost().ConfigureAwait(true);

        var values = new Dictionary<string, string>()
        {
            { "id", "4" },
            { "title", "My Title" },
            { "status", "C" },
        };

        using var content = new FormUrlEncodedContent(values);
        var targetUrl = new Uri($"{TestingHost.GetRequiredInstance().Address}/samplebooksbystringstatus/edit/1");
        using var response = await client.PostAsync(targetUrl, content).ConfigureAwait(true);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
        var book = JsonConvert.DeserializeObject<SampleBookByStringStatus>(responseContent, this.options);

        Assert.AreEqual(SampleStatusByString.Deleted, book?.Status);
    }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing).
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!hasDisposed)
        {
            if (disposing)
            {
                client.Dispose();
            }

            hasDisposed = true;
        }
    }
}