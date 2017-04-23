using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class ProductBrandAttr : BaseEntity
    {
        [DataMember]
        public int IdProductBrand { get; set; }

        [DataMember(IsRequired = true)]
        public string IdAppLanguage { get; set; }

        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Photo { get; set; }

        [DataMember(IsRequired = true)]
        public string Search { get; set; }

    }
}
