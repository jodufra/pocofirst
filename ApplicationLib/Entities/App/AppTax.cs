using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;
using Application.Repositories;

namespace Application.Entities
{
    [DataContract]
    public class AppTax : BaseEntity, ILinqExtent
    {
        [DataMember(IsRequired = true)]
        public string Title { get; set; }
        [DataMember]
        public decimal Tax { get; set; }
    }
}
