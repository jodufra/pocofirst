using System.Linq;
using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class EntityType : BaseEntity, ILinqExtent
    {
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
    }
}
