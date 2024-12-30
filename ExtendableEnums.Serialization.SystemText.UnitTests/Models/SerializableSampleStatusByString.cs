using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ExtendableEnums.Serialization.SystemText.UnitTests.Models;

[JsonConverter(typeof(ExtendableEnumJsonConverter))]
internal class SerializableSampleStatusByString : ExtendableEnumBase<SerializableSampleStatusByString, string>
{
    public static readonly SerializableSampleStatusByString Unknown = new("A", nameof(Unknown), "???");
    public static readonly SerializableSampleStatusByString Active = new("B", nameof(Active), "ACT");
    public static readonly SerializableSampleStatusByString Deleted = new("C", nameof(Deleted), "DEL");
    public static readonly SerializableSampleStatusByString Discontinued = new("D", nameof(Discontinued), "DIS");
    public static readonly SerializableSampleStatusByString Inactive = new("E", nameof(Inactive), "INA");

    private SerializableSampleStatusByString(string value, string displayName, string code)
        : base(value, displayName)
    {
        Code = code;
    }

    [NotMapped]
    public string Code { get; }
}