using System.Runtime.Serialization;

namespace ExtendableEnums.Testing.Models;

[DataContract]
public class SampleBook : SampleBookBase
{
    [DataMember(Name = "status")]
    public SampleStatus? Status { get; set; }
}