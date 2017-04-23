using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class ProductPhotoAttr : BaseEntity
    {
        [DataMember]
        public int IdProductPhoto { get; set; }

        [DataMember(IsRequired = true)]
        public string IdAppLanguage { get; set; }

        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }
        
    }
}
