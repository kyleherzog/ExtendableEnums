namespace ExtendableEnums.Serialization.SystemText.UnitTests.Models;

internal static class SerializableSampleStatusDeclared
{
    public static readonly SerializableSampleStatus Pending = SerializableSampleStatus.Extend(99, nameof(Pending), "PEN");
    public static readonly int SomeIntValue = 99;
}