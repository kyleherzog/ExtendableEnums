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
            var results = new List<SampleBook>();
            var itemCount = 20;
            for (var i = 0; i < itemCount; i++)
            {
                results.Add(new SampleBook
                {
                    Id = i.ToString(),
                    Title = $"Book #{i}",
                    Status = i % 2 == 0 ? SampleStatus.Deleted : SampleStatus.Active 
                });
            }

            return results;
        }
    }
}
