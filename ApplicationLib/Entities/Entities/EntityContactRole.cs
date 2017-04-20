using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;
using Application.Repositories;

namespace Application.Entities
{
    [DataContract]
    public class EntityContactRole : BaseEntity, ILinqExtent
    {
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
        [DataMember]
        public bool IsAdministrator { get; set; }

        public IList<AppArea> _Areas = null;
        public IList<AppArea> Areas
        {
            get
            {
                if (_Areas == null)
                    _Areas =  Configuration.Resolver.GetRepository<EntityContactRoleVsAppAreaRepository>().GetAreasByRole(this.Id);
                return _Areas;
            }
        }
    }
}
