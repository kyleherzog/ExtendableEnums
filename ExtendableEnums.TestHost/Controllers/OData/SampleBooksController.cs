using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ExtendableEnums.Testing.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Newtonsoft.Json;

namespace ExtendableEnums.TestHost.Controllers.OData;

public class SampleBooksController : ODataController
{
    private readonly IList<SampleBook> books = DataContext.Books;

    [HttpGet]
    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(books);
    }

    [HttpGet("odata/SampleBooks({id})")]
    [EnableQuery]
    public IActionResult Get(string id)
    {
        var book = books.FirstOrDefault(x => x.Id == id);
        if (book is null)
        {
            return NotFound($"Cannot find book with Id='{id}'.");
        }

        return Ok(book);
    }

    [HttpPost]
    [EnableQuery]
    public IActionResult Post([FromBody] JsonElement json)
    {
        var book = JsonConvert.DeserializeObject<SampleBook>(json.GetRawText());

        if (book is null)
        {
            throw new ArgumentException("Unable to deserialize the json parameter.", nameof(json));
        }

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