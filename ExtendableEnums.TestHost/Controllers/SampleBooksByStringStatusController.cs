using System;
using ExtendableEnums.Testing.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExtendableEnums.TestHost.Controllers
{
    public class SampleBooksByStringStatusController : Controller
    {
        public IActionResult Index()
        {
            return Ok("OK");
        }

        [HttpPost]
        public IActionResult Edit(int id, SampleBookByStringStatus model)
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
    }
}
