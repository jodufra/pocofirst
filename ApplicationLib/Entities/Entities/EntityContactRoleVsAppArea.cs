using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class EntityContactRoleVsAppArea : BaseEntity
    {
        [DataMember]
        public int IdEntityContactRole { get; set; }

        [DataMember]
        public int IdAppArea { get; set; }

    }
}
