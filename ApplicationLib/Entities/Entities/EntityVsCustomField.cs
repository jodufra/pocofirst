using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class EntityVsCustomField : CustomFieldEntity
    {
        [DataMember]
        public int IdEntity { get; set; }

    }
}
