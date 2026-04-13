using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ExtendableEnums.Serialization.Newtonsoft.UnitTests.Models;

[JsonConverter(typeof(ExtendableEnumJsonConverter))]
internal class SerializableSampleStatus : ExtendableEnums.ExtendableEnum<SerializableSampleStatus>
{
    public static readonly SerializableSampleStatus Active = new(1, nameof(Active), "ACT");
    public static readonly SerializableSampleStatus Deleted = new(2, nameof(Deleted), "DEL");
    public static readonly SerializableSampleStatus Discontinued = new(2, nameof(Discontinued), "DIS");
    public static readonly SerializableSampleStatus Inactive = new(3, nameof(Inactive), "INA");
    public static readonly SerializableSampleStatus Unknown = new(0, nameof(Unknown), "???");

    private SerializableSampleStatus(int value, string displayName, string code)
        : base(value, displayName)
    {
        Code = code;
    }

    [NotMapped]
    public string Code { get; }

    public static SerializableSampleStatus Extend(int value, string displayName, string code)
    {
        return new SerializableSampleStatus(value, displayName, code);
    }
}