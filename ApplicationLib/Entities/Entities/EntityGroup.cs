using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class EntityGroup : BaseEntity, ILinqExtent
    {
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
    }
}
