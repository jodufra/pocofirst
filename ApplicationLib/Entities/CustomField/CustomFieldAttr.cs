using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;
using Application.Repositories;

namespace Application.Entities
{
    [DataContract]
    public class CustomFieldAttr : BaseEntity, ILinqExtent
    {
        [DataMember]
        public int IdCustomField { get; set; }
        [DataMember(IsRequired = true)]
        public string IdAppLanguage { get; set; }
        [DataMember(IsRequired = true)]
        public string Title { get; set; }
        [DataMember(IsRequired = true)]
        public string Search { get; set; }

        private CustomField _CustomField = null;
        public CustomField CustomField
        {
            get
            {
                if (_CustomField == null && IdCustomField > 0)
                    _CustomField = Configuration.Resolver.GetRepository<CustomFieldRepository>().GetById(this.IdCustomField);
                return _CustomField;
            }
        }
    }
}
