using System;
using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class EntityContact : BaseEntity
    {
        [DataMember]
        public int IdEntity { get; set; }

        [DataMember]
        public int? IdEntityContactRole { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Position { get; set; }

        [DataMember]
        public string Contact { get; set; }

        [DataMember]
        public string Notes { get; set; }

        [DataMember]
        public string IdAppLanguage { get; set; }

        [DataMember]
        public bool IsUser { get; set; }

        [DataMember]
        public string Photo { get; set; }

        [DataMember]
        public DateTime? DateLogin { get; set; }
        
    }
}
