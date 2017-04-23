using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class ProductAttr : BaseEntity
    {
        [DataMember]
        public int IdProduct { get; set; }

        [DataMember(IsRequired = true)]
        public string IdAppLanguage { get; set; }

        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        [DataMember]
        public string Introduction { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string SEOKeywords { get; set; }

        [DataMember]
        public string SEODescription { get; set; }

        [DataMember(IsRequired = true)]
        public string Search { get; set; }

    }
}
