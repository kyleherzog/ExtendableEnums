using System.Runtime.Serialization;

namespace ExtendableEnums.Testing.Models;

[DataContract]
public class SampleBookByStringStatus
{
    [DataMember(Name = "id")]
    public string? Id { get; set; }

    [DataMember(Name = "status")]
    public SampleStatusByString? Status { get; set; }

    [DataMember(Name = "title")]
    public string? Title { get; set; }
}