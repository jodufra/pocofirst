using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class AppSetting : BaseEntity
    {
        [DataMember(IsRequired = true)]
        public string Reference { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public bool IsCore { get; set; }

        [DataMember]
        public int TypeId { get; set; }
    }

    public class AppSettingType
    {
        public const int TEXT = 0;
        public const int NUMBER = 1;
        public const int DATE = 2;
        public const int BOOL = 3;
    }
}
