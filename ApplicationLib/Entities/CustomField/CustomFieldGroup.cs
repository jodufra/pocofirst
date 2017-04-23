using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class CustomFieldGroup : BaseEntity, IEntityWithAttr
    {
        [DataMember]
        public int OrderId { get; set; }

    }
}
