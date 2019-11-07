using System.ComponentModel.DataAnnotations.Schema;

namespace ExtendableEnums.Testing.Models
{
    public class SampleStatusByString : ExtendableEnumBase<SampleStatusByString, string>
    {
        public static readonly SampleStatusByString Unknown = new SampleStatusByString("A", nameof(Unknown), "???");
        public static readonly SampleStatusByString Active = new SampleStatusByString("B", nameof(Active), "ACT");
        public static readonly SampleStatusByString Deleted = new SampleStatusByString("C", nameof(Deleted), "DEL");
        public static readonly SampleStatusByString Discontinued = new SampleStatusByString("D", nameof(Discontinued), "DIS");
        public static readonly SampleStatusByString Inactive = new SampleStatusByString("E", nameof(Inactive), "INA");

        private SampleStatusByString(string value, string displayName, string code)
            : base(value, displayName)
        {
            Code = code;
        }

        [NotMapped]
        public string Code { get; }
    }
}
