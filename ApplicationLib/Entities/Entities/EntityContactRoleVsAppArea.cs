using System.Linq;
using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class EntityContactRoleVsAppArea : BaseEntity, ILinqExtent
    {
        [DataMember]
        public int IdEntityContactRole { get; set; }
        [DataMember]
        public int IdAppArea { get; set; }
    }
}
