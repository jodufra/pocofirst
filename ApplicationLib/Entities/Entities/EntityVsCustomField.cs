using System.Linq;
using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class EntityVsCustomField : CustomFieldEntity, ILinqExtent
    {
        [DataMember]
        public int IdEntity { get; set; }
    }
}
