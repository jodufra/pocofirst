using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace Application.Entities
{

    [DataContract]
    public class BaseEntity : AEntity
    {
        [DataMember]
        public override int Id { get; set; }
        [DataMember]
        public override DateTime DateCreated { get; set; }
        [DataMember]
        public override DateTime? DateUpdated { get; set; }
    }

    [DataContract]
    public class CustomFieldEntity : BaseEntity, IEntityCustomFieldRelation
    {
        [DataMember]
        public int IdCustomField { get; set; }

        [DataMember]
        public string Reference { get; set; }

        [DataMember]
        public string IdAppLanguage { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}
