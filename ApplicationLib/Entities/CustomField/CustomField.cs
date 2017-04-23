using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class CustomField : BaseEntity, IEntityWithAttr
    {
        [DataMember]
        public int IdCustomFieldType { get; set; }

        [DataMember]
        public int? IdCustomFieldGroup { get; set; }

        [DataMember]
        public int? IdEntityType { get; set; }

        [DataMember]
        public int OrderId { get; set; }

        [DataMember(IsRequired = true)]
        public string Reference { get; set; }

        [DataMember]
        public string Default { get; set; }

        [DataMember]
        public bool IsVisibleInEntities { get; set; }

        [DataMember]
        public bool IsVersionAttribute { get; set; }

        [DataMember]
        public bool IsVisibleInProducts { get; set; }
        
    }
}
