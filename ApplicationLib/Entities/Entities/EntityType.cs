using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class EntityType : BaseEntity
    {
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

    }
}
