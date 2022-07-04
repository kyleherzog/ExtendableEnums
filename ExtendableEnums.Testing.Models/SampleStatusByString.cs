using System.ComponentModel.DataAnnotations.Schema;
using ExtendableEnums.Serialization.SystemText;

namespace ExtendableEnums.Testing.Models;

[System.Text.Json.Serialization.JsonConverter(typeof(ExtendableEnumJsonConverter))]
public class SampleStatusByString : ExtendableEnumBase<SampleStatusByString, string>
{
    public static readonly SampleStatusByString Unknown = new("A", nameof(Unknown), "???");
    public static readonly SampleStatusByString Active = new("B", nameof(Active), "ACT");
    public static readonly SampleStatusByString Deleted = new("C", nameof(Deleted), "DEL");
    public static readonly SampleStatusByString Discontinued = new("D", nameof(Discontinued), "DIS");
    public static readonly SampleStatusByString Inactive = new("E", nameof(Inactive), "INA");

    private SampleStatusByString(string value, string displayName, string code)
        : base(value, displayName)
    {
        Code = code;
    }

    [NotMapped]
    public string Code { get; }
}