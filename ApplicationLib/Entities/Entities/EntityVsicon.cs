using System.Linq;
using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class EntityVsIcon : BaseEntity, ILinqExtent
    {
        [DataMember]
        public int IdEntity { get; set; }
        [DataMember]
        public int IdIcon { get; set; }
    }
}
