using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtendableEnums.Testing.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace ExtendableEnums.OData.TestHost.Controllers
{

    public class SampleBooksController : ODataController
    {
        [EnableQuery]
        public IEnumerable<SampleBook> Get()
        {
            return DataContext.Books;
        }
    }
}
