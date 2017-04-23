using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class EntityAddress : BaseEntity
    {
        [DataMember]
        public int IdEntity { get; set; }

        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        [DataMember]
        public string AddressLine { get; set; }

        [DataMember]
        public string AddressLocation { get; set; }

        [DataMember]
        public string AddressPostCode { get; set; }

        [DataMember]
        public string AddressDistrict { get; set; }

        [DataMember]
        public string IdAppCountry { get; set; }

        [DataMember]
        public bool IsDefault { get; set; }

    }
}
