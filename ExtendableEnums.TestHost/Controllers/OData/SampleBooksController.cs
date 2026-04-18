using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using ExtendableEnums.Testing.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace ExtendableEnums.TestHost.Controllers.OData;

[SuppressMessage("Major Code Smell", "S6934:A Route attribute should be added to the controller when a route template is specified at the action level", Justification = "Adding a route attribute breaks OData routing.")]
public class SampleBooksController : ODataController
{
    private readonly IList<SampleBook> books = DataContext.Books;

    [HttpGet]
    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(books);
    }

    [HttpGet]
    [Route("odata/SampleBooks({id})")]
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
        var book = JsonSerializer.Deserialize<SampleBook>(json.GetRawText())
            ?? throw new ArgumentException("Unable to deserialize the json parameter.", nameof(json));

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