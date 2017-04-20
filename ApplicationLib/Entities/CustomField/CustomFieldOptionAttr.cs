using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;
using Application.Repositories;

namespace Application.Entities
{
    [DataContract]
    public class CustomFieldOptionAttr : BaseEntity, ILinqExtent
    {
        [DataMember]
        public int IdCustomFieldOption { get; set; }

        [DataMember(IsRequired = true)]
        public string IdAppLanguage { get; set; }

        [DataMember(IsRequired = true)]
        public string Value { get; set; }

        private CustomFieldOption _Option = null;
        public CustomFieldOption Option
        {
            get
            {
                if (_Option == null && IdCustomFieldOption > 0)
                    _Option = Configuration.Resolver.GetRepository<CustomFieldOptionRepository>().GetById(this.IdCustomFieldOption);
                return _Option;
            }
        }
    }
}
