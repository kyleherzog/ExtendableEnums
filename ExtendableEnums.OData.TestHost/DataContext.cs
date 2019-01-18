using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ExtendableEnums.Testing.Models;

namespace ExtendableEnums.OData.TestHost
{
    public static class DataContext
    {
        public static IList<SampleBook> Books { get; set; }

        public static void ResetData()
        {
            ResetBooks();
        }

        private static void ResetBooks()
        {
            Books = new List<SampleBook>();
            var itemCount = 20;
            for (var i = 0; i < itemCount; i++)
            {
                Books.Add(new SampleBook
                {
                    Id = i.ToString(CultureInfo.InvariantCulture),
                    Title = $"Book #{i}",
                    Status = i % 2 == 0 ? SampleStatus.Deleted : SampleStatus.Active
                });
            }
        }
    }
}
