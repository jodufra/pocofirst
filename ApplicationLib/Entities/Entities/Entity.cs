using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class Entity : BaseEntity
    {
        [DataMember]
        public int? IdEntityType { get; set; }

        [DataMember]
        public int? IdEntityGroup { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember]
        public string FiscalId { get; set; }

        [DataMember]
        public string Photo { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string ContactDefault { get; set; }

        [DataMember]
        public string ContactOptional { get; set; }

        [DataMember]
        public string Website { get; set; }

        [DataMember]
        public string Notes { get; set; }

        [DataMember(IsRequired = true)]
        public string Search { get; set; }
        
    }
}
