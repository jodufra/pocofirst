using System.Linq;
using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class ProductVsIcon : BaseEntity, ILinqExtent
    {
        [DataMember]
        public int IdProduct { get; set; }
        [DataMember]
        public int IdIcon { get; set; }
    }
}
