using System;
using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class Product : BaseEntity, IEntityWithAttr
    {
        [DataMember]
        public int IdAppTax { get; set; }

        [DataMember]
        public int? IdProductBrand { get; set; }

        [DataMember(IsRequired = true)]
        public string Reference { get; set; }

        [DataMember]
        public bool HasReviews { get; set; }

        [DataMember]
        public bool IsHighlight { get; set; }

        [DataMember]
        public DateTime? DatePublished { get; set; }
        
    }
}
