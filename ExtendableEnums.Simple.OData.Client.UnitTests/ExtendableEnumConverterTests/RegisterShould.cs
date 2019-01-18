using System;
using System.Linq;
using System.Threading.Tasks;
using ExtendableEnums.OData.TestHost;
using ExtendableEnums.SimpleOData.Client;
using ExtendableEnums.SimpleOData.Client.UnitTests;
using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simple.OData.Client;

namespace ExtendableEnums.Simple.OData.Client.UnitTests.ExtendableEnumConverterTests
{
    [TestClass]
    public class RegisterShould
    {
        [TestMethod]
        public async Task RegisterConverterGivenGenericMethodCalled()
        {
            await TestingHost.Instance.GetNewWebHost().ConfigureAwait(true);
            var settings = new ODataClientSettings
            {
                BaseUri = TestingHost.Instance.BaseODataUrl
            };

            ExtendableEnumConverter.Register<SampleStatus>(settings);
            var client = new ODataClient(settings);

            var target = DataContext.Books.First();

            var book = await client
                .For<SampleBook>()
                .Key(target.Id)
                .FindEntryAsync()
                .ConfigureAwait(true);

            Assert.AreEqual(target.Status, book.Status);
        }

        [TestMethod]
        public async Task RegisterConverterGivenNonGenericMethodCalled()
        {
            await TestingHost.Instance.GetNewWebHost().ConfigureAwait(true);
            var settings = new ODataClientSettings
            {
                BaseUri = TestingHost.Instance.BaseODataUrl
            };

            ExtendableEnumConverter.Register(typeof(SampleStatus), settings);
            var client = new ODataClient(settings);

            var target = DataContext.Books.First();

            var book = await client
                .For<SampleBook>()
                .Key(target.Id)
                .FindEntryAsync()
                .ConfigureAwait(true);

            Assert.AreEqual(target.Status, book.Status);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ThrowArgumentExceptionGivenTypeIsNotExtendableEnumDescendant()
        {
            await TestingHost.Instance.GetNewWebHost().ConfigureAwait(true);
            var settings = new ODataClientSettings
            {
                BaseUri = TestingHost.Instance.BaseODataUrl
            };

            ExtendableEnumConverter.Register(typeof(string), settings);
            Assert.Fail("An ArgumentException should have been thrown.");
        }
    }
}
