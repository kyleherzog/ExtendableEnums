﻿using System.Globalization;
using ExtendableEnums.Testing.Models;

namespace ExtendableEnums.TestHost;

public static class DataContext
{
    public static IList<SampleBook> Books { get; } = new List<SampleBook>();

    public static void ResetData()
    {
        ResetBooks();
    }

    private static void ResetBooks()
    {
        Books.Clear();
        var itemCount = 20;
        for (var i = 0; i < itemCount; i++)
        {
            Books.Add(new SampleBook
            {
                Id = i.ToString(CultureInfo.InvariantCulture),
                Title = $"Book #{i}",
                Status = i % 2 == 0 ? SampleStatus.Deleted : SampleStatus.Active,
            });
        }

        Books[Books.Count - 1].Status = null;
    }
}