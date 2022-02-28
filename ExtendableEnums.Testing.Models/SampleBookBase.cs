using System.Runtime.Serialization;

namespace ExtendableEnums.Testing.Models;

[DataContract]
public class SampleBookBase
{
    [DataMember(Name = "id")]
    public string? Id { get; set; }

    [DataMember(Name = "title")]
    public string? Title { get; set; }
}