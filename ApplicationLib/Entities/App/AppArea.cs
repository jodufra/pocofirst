using System.Linq;
using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class AppArea : BaseEntity
    {
        [DataMember(IsRequired = true)]
        public string Reference { get; set; }

        [DataMember(IsRequired = true)]
        public string Icon { get; set; }

        [DataMember(IsRequired = true)]
        public string Color { get; set; }

        [DataMember(IsRequired = true)]
        public string Route { get; set; }

        [DataMember(IsRequired = true)]
        public int OrderId { get; set; }
    }
}
