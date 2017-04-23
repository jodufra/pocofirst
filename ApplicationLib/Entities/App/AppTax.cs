using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class AppTax : BaseEntity
    {
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        [DataMember]
        public decimal Tax { get; set; }
    }
}
