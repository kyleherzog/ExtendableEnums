using System.Runtime.Serialization;

namespace ExtendableEnums.Testing.Models;

[DataContract]
public class SampleBook
{
    [DataMember(Name = "id")]
    public string Id { get; set; }

    [DataMember(Name = "status")]
    public SampleStatus Status { get; set; }

    [DataMember(Name = "title")]
    public string Title { get; set; }
}