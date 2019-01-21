using System.Collections.Generic;
using System.Linq;
using ExtendableEnums.Testing.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace ExtendableEnums.OData.TestHost.Controllers
{
    public class SampleBooksController : ODataController
    {
        private readonly IList<SampleBook> books = DataContext.Books;

        [EnableQuery]
        public IEnumerable<SampleBook> Get()
        {
            return books;
        }

        [EnableQuery]
        public IActionResult Post([FromBody] JObject json)
        {
            var book = json.ToObject<SampleBook>();
            var matchingBook = books.FirstOrDefault(b => b.Id == book.Id);
            if (matchingBook == null)
            {
                books.Add(book);
                return Created(book);
            }
            else
            {
                var index = books.IndexOf(matchingBook);
                books.RemoveAt(index);
                books.Insert(index, book);
                return Updated(book);
            }
        }
    }
}
