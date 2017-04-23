using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class ProductPhoto : BaseEntity, IEntityWithAttr
    {
        [DataMember]
        public int IdProduct { get; set; }

        [DataMember]
        public int OrderId { get; set; }

        [DataMember(IsRequired = true)]
        public string Filename { get; set; }
        
    }
}
