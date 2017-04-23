using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class ProductVsIcon : BaseEntity
    {
        [DataMember]
        public int IdProduct { get; set; }

        [DataMember]
        public int IdIcon { get; set; }
    }
}
