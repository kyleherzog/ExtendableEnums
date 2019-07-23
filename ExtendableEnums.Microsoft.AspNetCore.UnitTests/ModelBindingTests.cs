using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ExtendableEnums.Microsoft.AspNetCore.UnitTests
{
    [TestClass]
    public class ModelBindingTests
    {
        private static readonly HttpClient client = new HttpClient();

        [TestMethod]
        public async Task BindTheExtendableEnumCorrectly()
        {
            await TestingHost.Instance.GetNewWebHost().ConfigureAwait(true);

            var values = new Dictionary<string, string>()
            {
                { "id", "4" },
                { "title", "My Title" },
                { "status", "2" }
            };

            var content = new FormUrlEncodedContent(values);
            var targetUrl = $"{TestingHost.Instance.Address}/samplebooks/edit/1";
            var response = await client.PostAsync(targetUrl, content);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var book = JsonConvert.DeserializeObject<SampleBook>(responseContent);

            Assert.AreEqual(SampleStatus.Deleted, book.Status);
        }
    }
}
