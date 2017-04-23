using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class CustomFieldOption : BaseEntity, IEntityWithAttr
    {
        [DataMember]
        public int IdCustomField { get; set; }

        [DataMember]
        public int Index { get; set; }
        
    }
}
