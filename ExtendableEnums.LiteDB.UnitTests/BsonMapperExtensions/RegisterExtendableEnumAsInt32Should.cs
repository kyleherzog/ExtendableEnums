using System;
using System.IO;
using ExtendableEnums.Testing.Models;
using FluentAssertions;
using LiteDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendableEnums.LiteDB.UnitTests.BsonMapperExtensions
{
    [TestClass]
    public class RegisterExtendableEnumAsInt32Should
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowArgumentExceptionGivenTypeIsNotExtendableEnum()
        {
            BsonMapper.Global.RegisterExtendableEnumAsInt32(typeof(string));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowArgumentExceptionGivenValueTypeOfExtendableEnumIsNotInt()
        {
            BsonMapper.Global.RegisterExtendableEnumAsInt32(typeof(SampleStatusByString));
        }

        [TestMethod]
        public void RegisterCorrectlyGivenProperType()
        {
            var mapper = new BsonMapper();
            mapper.RegisterExtendableEnumAsInt32(typeof(SampleStatus));

            var fileName = Path.GetTempFileName();
            try
            {
                using (var db = new LiteDatabase($"Filename={fileName}", mapper))
                {
                    var book = new SampleBook
                    {
                        Status = SampleStatus.Deleted,
                        Id = Guid.NewGuid().ToString(),
                        Title = "The Greatest Book in the World",
                    };

                    var collection = db.GetCollection<SampleBook>();
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
