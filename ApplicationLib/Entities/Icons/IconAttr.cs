using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;
using Application.Repositories;

namespace Application.Entities
{
    [DataContract]
    public class IconAttr : BaseEntity, ILinqExtent
    {
        [DataMember]
        public int IdIcon { get; set; }
        [DataMember(IsRequired = true)]
        public string IdAppLanguage { get; set; }
        [DataMember(IsRequired = true)]
        public string Title { get; set; }
      
    }
}
