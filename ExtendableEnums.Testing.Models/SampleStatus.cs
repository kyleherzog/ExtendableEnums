using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ExtendableEnums.Testing.Models
{
    public class SampleStatus : ExtendableEnums.ExtendableEnum<SampleStatus>
    {
        public static readonly SampleStatus Active = new SampleStatus(1, nameof(Active), "ACT");
        public static readonly SampleStatus Deleted = new SampleStatus(2, nameof(Deleted), "DEL");
        public static readonly SampleStatus Discontinued = new SampleStatus(2, nameof(Discontinued), "DIS");
        public static readonly SampleStatus Inactive = new SampleStatus(3, nameof(Inactive), "INA");
        public static readonly SampleStatus Bogus = new SampleStatus(4, null, null);

        public SampleStatus() 
            : base()
        {
        }

        private SampleStatus(int value, string displayName, string code)
            : base(value, displayName)
        {
            Code = code;
        }

        [NotMapped]
        public string Code { get; private set; }
    }
}
