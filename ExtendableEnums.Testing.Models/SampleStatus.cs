using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExtendableEnums.Testing.Models
{
    public class SampleStatus : ExtendableEnums.ExtendableEnum<SampleStatus>
    {
        public static readonly SampleStatus Unknown = new SampleStatus(0, nameof(Unknown), "???");
        public static readonly SampleStatus Active = new SampleStatus(1, nameof(Active), "ACT");
        public static readonly SampleStatus Deleted = new SampleStatus(2, nameof(Deleted), "DEL");
        public static readonly SampleStatus Discontinued = new SampleStatus(2, nameof(Discontinued), "DIS");
        public static readonly SampleStatus Inactive = new SampleStatus(3, nameof(Inactive), "INA");

        private SampleStatus(int value, string displayName, string code)
            : base(value, displayName)
        {
            Code = code;
        }

        [NotMapped]
        public string Code { get; }
    }
}
