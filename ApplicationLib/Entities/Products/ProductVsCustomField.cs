using System.Linq;
using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class ProductVsCustomField : CustomFieldEntity, ILinqExtent
    {
        [DataMember]
        public int IdProduct { get; set; }
    }
}
