using System;
using ExtendableEnums.Testing.Models;

namespace ExtendableEnums.TestHost.Models;

public class SampleBooksModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public SampleStatus? Status { get; set; }
}