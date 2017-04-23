using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class CustomFieldGroupAttr : BaseEntity
    {
        [DataMember]
        public int IdCustomFieldGroup { get; set; }

        [DataMember(IsRequired = true)]
        public string IdAppLanguage { get; set; }

        [DataMember(IsRequired = true)]
        public string Title { get; set; }

    }
}
