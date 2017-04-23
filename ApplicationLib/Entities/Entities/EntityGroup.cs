using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class EntityGroup : BaseEntity
    {
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

    }
}
