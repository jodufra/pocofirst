using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class ProductBrand : BaseEntity, IEntityWithAttr
    {
        [DataMember]
        public int OrderId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsHighlight { get; set; }
        
    }
}
