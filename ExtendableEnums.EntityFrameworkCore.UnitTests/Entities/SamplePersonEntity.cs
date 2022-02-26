using System.ComponentModel.DataAnnotations;
using ExtendableEnums.Testing.Models;

namespace ExtendableEnums.EntityFrameworkCore.UnitTests.Entities;

public class SamplePersonEntity
{
    [Key]
    public string? Id { get; set; }

    public string? Name { get; set; }

    public SampleStatus? Status { get; set; }
}