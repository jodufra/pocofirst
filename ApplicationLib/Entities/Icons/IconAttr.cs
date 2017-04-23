using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class IconAttr : BaseEntity
    {
        [DataMember]
        public int IdIcon { get; set; }

        [DataMember(IsRequired = true)]
        public string IdAppLanguage { get; set; }

        [DataMember(IsRequired = true)]
        public string Title { get; set; }
      
    }
}
