namespace ExtendableEnums.UnitTests.Models
{
    public class SampleStatus : ExtendableEnums.ExtendableEnum<SampleStatus>
    {
        public static readonly SampleStatus Active = new SampleStatus(1, nameof(Active), "ACT");
        public static readonly SampleStatus Discontinued = new SampleStatus(2, nameof(Discontinued), "DIS");
        public static readonly SampleStatus Inactive = new SampleStatus(3, nameof(Inactive), "INA");

        private SampleStatus(int value, string displayName, string code)
            : base(value, displayName)
        {
            Code = code;
        }

        public string Code { get; private set; }
    }
}
