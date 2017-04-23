using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class CustomFieldAttr : BaseEntity
    {
        [DataMember]
        public int IdCustomField { get; set; }

        [DataMember(IsRequired = true)]
        public string IdAppLanguage { get; set; }

        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        [DataMember(IsRequired = true)]
        public string Search { get; set; }

    }
}
