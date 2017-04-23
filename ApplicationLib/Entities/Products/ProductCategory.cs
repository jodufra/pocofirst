using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class ProductCategory : BaseEntity, IEntityWithAttr
    {
        [DataMember]
        public int? IdParent { get; set; }

        [DataMember(IsRequired = true)]
        public string Path { get; set; }

        [DataMember]
        public int OrderId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsHighlight { get; set; }
        
    }
}
