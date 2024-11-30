using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExtendableEnums.Testing.Models;

namespace ExtendableEnums.EntityFrameworkCore.UnitTests.Entities;

public class SamplePersonEntity
{
    [Key]
    public string? Id { get; set; }

    public string? Name { get; set; }

    public SampleStatus? Status { get; set; }

    [NotMapped]
    public SampleStatus? NotMapppedStatus { get => Status; set => Status = value; }

    public SampleStatus? IgnoredStatus { get => Status; set => Status = value; }
}