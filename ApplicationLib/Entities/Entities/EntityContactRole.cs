using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class EntityContactRole : BaseEntity
    {
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember]
        public bool IsAdministrator { get; set; }

    }
}
