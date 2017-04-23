using System;
using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class AppCountry : IEntityWithAttr
    {
        [DataMember(IsRequired = true)]
        public string Id { get; set; }

        [DataMember]
        public bool IncludeTax { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public DateTime? DateCreated { get; set; }

        [DataMember]
        public DateTime? DateUpdated { get; set; }

        public bool IsNew()
        {
            return !DateCreated.HasValue;
        }
    }
}
