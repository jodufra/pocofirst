using System;
using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class Event : BaseEntity
    {
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime DateStart { get; set; }

        [DataMember]
        public DateTime DateEnd { get; set; }

    }
}
