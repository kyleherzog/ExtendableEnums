using System.ComponentModel.DataAnnotations.Schema;

namespace ExtendableEnums.Testing.Models;

public class SampleStatus : ExtendableEnums.ExtendableEnum<SampleStatus>
{
    public static readonly SampleStatus Active = new(1, nameof(Active), "ACT");
    public static readonly SampleStatus Deleted = new(2, nameof(Deleted), "DEL");
    public static readonly SampleStatus Discontinued = new(2, nameof(Discontinued), "DIS");
    public static readonly SampleStatus Inactive = new(3, nameof(Inactive), "INA");
    public static readonly SampleStatus Unknown = new(0, nameof(Unknown), "???");

    private SampleStatus(int value, string displayName, string code)
        : base(value, displayName)
    {
        Code = code;
    }

    [NotMapped]
    public string Code { get; }

    public static SampleStatus Extend(int value, string displayName, string code)
    {
        return new SampleStatus(value, displayName, code);
    }
}