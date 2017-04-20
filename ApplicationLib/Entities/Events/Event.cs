using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities
{
    [DataContract]
    public class Event : BaseEntity, ILinqExtent
    {
        [DataMember(IsRequired = true)]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime DateStart { get; set; }
        [DataMember]
        public DateTime DateEnd { get; set; }

        public Event() { }

    }
}
