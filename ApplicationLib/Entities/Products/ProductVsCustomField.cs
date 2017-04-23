using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class ProductVsCustomField : CustomFieldEntity
    {
        [DataMember]
        public int IdProduct { get; set; }
    }
}
