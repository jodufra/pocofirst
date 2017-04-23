using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class CustomFieldOptionAttr : BaseEntity
    {
        [DataMember]
        public int IdCustomFieldOption { get; set; }

        [DataMember(IsRequired = true)]
        public string IdAppLanguage { get; set; }

        [DataMember(IsRequired = true)]
        public string Value { get; set; }

    }
}
