using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ExtendableEnums.Testing.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExtendableEnums.TestHost.Controllers.OData;

public class SampleBooksController : ODataController
{
    private readonly IList<SampleBook> books = DataContext.Books;

    [EnableQuery]
    public IEnumerable<SampleBook> Get()
    {
        return books;
    }

    [EnableQuery]
    public IActionResult Post([FromBody] JsonElement json)
    {
        if (json.Equals(default))
        {
            throw new ArgumentNullException(nameof(json));
        }

        var book = JsonConvert.DeserializeObject<SampleBook>(json.GetRawText());

        var matchingBook = books.FirstOrDefault(b => b.Id == book.Id);
        if (matchingBook is null)
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