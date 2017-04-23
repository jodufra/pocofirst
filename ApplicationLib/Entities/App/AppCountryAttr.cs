using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class AppCountryAttr : BaseEntity
    {
        [DataMember(IsRequired = true)]
        public string IdAppCountry { get; set; }

        [DataMember(IsRequired = true)]
        public string IdAppLanguage { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }
    }
}
