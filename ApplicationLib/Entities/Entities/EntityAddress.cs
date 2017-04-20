using System.Linq;
using System.Runtime.Serialization;
using Application.Repositories;
using System;

namespace Application.Entities
{
    [DataContract]
    public class EntityAddress : BaseEntity, ILinqExtent
    {
        [DataMember]
        public int IdEntity { get; set; }
        [DataMember(IsRequired = true)]
        public string Title { get; set; }
        [DataMember]
        public string AddressLine { get; set; }
        [DataMember]
        public string AddressLocation { get; set; }
        [DataMember]
        public string AddressPostCode { get; set; }
        [DataMember]
        public string AddressDistrict { get; set; }
        [DataMember]
        public string IdAppCountry { get; set; }
        [DataMember]
        public bool IsDefault { get; set; }

        private Entity _Entity = null;
        public Entity Entity
        {
            get
            {
                if (_Entity == null && IdEntity > 0)
                    _Entity = Configuration.Resolver.GetRepository<EntityRepository>().GetById(this.IdEntity);
                return _Entity;
            }
        }

        private AppCountry _Country = null;
        public AppCountry Country
        {
            get
            {
                if (_Country == null)
                    _Country =  Configuration.Resolver.GetRepository<AppCountryRepository>().GetISO(this.IdAppCountry);
                return _Country;
            }
        }
    }
}
