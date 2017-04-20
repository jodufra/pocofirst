using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;
using Application.Repositories;

namespace Application.Entities
{
    [DataContract]
    public class AppCountryAttr : BaseEntity, ILinqExtent
    {
        [DataMember(IsRequired = true)]
        public string IdAppCountry { get; set; }
        [DataMember(IsRequired = true)]
        public string IdAppLanguage { get; set; }
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
    }
}
