using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class ProductVsProductCategory : BaseEntity
    {
        [DataMember]
        public int IdProduct { get; set; }

        [DataMember]
        public int IdProductCategory { get; set; }

        [DataMember]
        public int OrderId { get; set; }
    }
}
