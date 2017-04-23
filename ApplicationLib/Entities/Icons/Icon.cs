using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class Icon : BaseEntity, IEntityWithAttr
    {
        [DataMember(IsRequired = true)]
        public string Filename { get; set; }

        [DataMember]
        public int OrderId { get; set; }
        
    }
}
