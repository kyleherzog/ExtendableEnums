namespace ExtendableEnums.Testing.Models
{
    public static class SampleStatusDeclared
    {
        public static readonly SampleStatus Pending = SampleStatus.Extend(99, nameof(Pending), "PEN");
        public static readonly int SomeIntValue = 99;
    }
}
