using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simple.OData.Client;

namespace ExtendableEnums.SimpleOData.Client.UnitTests.TypeConvertersTests
{
    [TestClass]
    public class ConvertExtendableEnumShould
    {
        [TestMethod]
        public async Task ConvertValueOnlyToSampleStatus()
        {
            await AssemblyInitializer.StaticTestingHost.GetNewWebHost().ConfigureAwait(true);
            var settings = new ODataClientSettings
            {
                BaseUri = AssemblyInitializer.StaticTestingHost.BaseODataUrl
            };

            ExtendableEnumConverter.Register<SampleStatus>(settings);
            var client = new ODataClient(settings);
            
            var book = await client
                .For<SampleBook>()
                .Key("1")
                .FindEntryAsync()
                .ConfigureAwait(true);

            Assert.AreEqual(SampleStatus.Deleted, book.Status);
        }

        [TestMethod]
        public async Task ConvertValueOnlyToSampleStatusGivenTypesPassed()
        {
            var dictionary = new Dictionary<string, object>
            {
                { "value", 1 }
            };

            var result = ExtendableEnumConverter.Convert(typeof(SampleStatus), typeof(int), dictionary);
            Assert.AreEqual(SampleStatus.Active, result);
        }
    }
}
