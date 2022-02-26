using System;
using ExtendableEnums.TestHost.Models;
using ExtendableEnums.Testing.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExtendableEnums.TestHost.Controllers;

public class SampleBooksController : Controller
{
    [HttpPost]
    public IActionResult Edit(int id, SampleBook model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        else
        {
            Console.WriteLine($"Editing book with id of {id}");
            return Ok(model);
        }
    }

    public IActionResult Index()
    {
        var model = new SampleBooksModel
        {
            Status = SampleStatus.Inactive,
        };

        return View(model);
    }
}