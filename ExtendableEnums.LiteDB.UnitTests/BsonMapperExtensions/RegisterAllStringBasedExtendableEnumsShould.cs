using System;
using System.IO;
using ExtendableEnums.Testing.Models;
using FluentAssertions;
using LiteDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.LiteDB.UnitTests.BsonMapperExtensions
{
    [TestClass]
    public class RegisterAllStringBasedExtendableEnumsShould
    {
        [TestMethod]
        public void RegisterAllStringBasedExtendableEnumsGivenAssemblyMarkerType()
        {
            var mapper = new BsonMapper();
            mapper.RegisterAllStringBasedExtendableEnums(typeof(SampleBook));

            var fileName = Path.GetTempFileName();
            try
            {
                using (var db = new LiteDatabase($"Filename={fileName}", mapper))
                {
                    var book = new SampleBookByStringStatus
                    {
                        Status = SampleStatusByString.Deleted,
                        Id = Guid.NewGuid().ToString(),
                        Title = "The Greatest Book in the World",
                    };

                    var collection = db.GetCollection<SampleBookByStringStatus>();
                    collection.Insert(book);

                    var bookFromDb = collection.FindOne(x => x.Id == book.Id);

                    bookFromDb.Should().BeEquivalentTo(book);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }
    }
}
