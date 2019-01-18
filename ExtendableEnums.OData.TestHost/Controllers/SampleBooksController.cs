using System.Collections.Generic;
using ExtendableEnums.Testing.Models;
using Microsoft.AspNet.OData;

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
