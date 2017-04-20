using System.Linq;
using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class ProductVsProductCategory : BaseEntity, ILinqExtent
    {
        [DataMember]
        public int IdProduct { get; set; }
        [DataMember]
        public int IdProductCategory { get; set; }
        [DataMember]
        public int OrderId { get; set; }
    }
}
