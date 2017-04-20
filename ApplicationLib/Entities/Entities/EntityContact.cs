using System;
using System.Linq;
using System.Runtime.Serialization;
using Application.Repositories;

namespace Application.Entities
{
    [DataContract]
    public class EntityContact : BaseEntity, ILinqExtent
    {
        [DataMember]
        public int IdEntity { get; set; }
        [DataMember]
        public int? IdEntityContactRole { get; set; }
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Position { get; set; }
        [DataMember]
        public string Contact { get; set; }
        [DataMember]
        public string Notes { get; set; }
        [DataMember]
        public string IdAppLanguage { get; set; }
        [DataMember]
        public bool IsUser { get; set; }
        [DataMember]
        public string Photo { get; set; }
        [DataMember]
        public DateTime? DateLogin { get; set; }

        public string EntityName
        {
            get
            {
                return Entity != null ? Entity.Name : null;
            }
        }

        public string GetPhotoSrc()
        {
            return string.IsNullOrEmpty(this.Photo) ? "#" : "/" + System.Configuration.ConfigurationManager.AppSettings["Folders:Users:Photos"] + "/" + this.Photo;
        }

        public bool IsUserAdministrator()
        {
            return IdEntityContactRole.HasValue && Role != null && Role.IsAdministrator;
        }

        public bool IsUserMaster()
        {
            return IsUserAdministrator() && IdEntity == 1;
        }

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

        private EntityContactRole _Role = null;
        public EntityContactRole Role
        {
            get
            {
                if (_Role == null && IdEntityContactRole.HasValue && IdEntityContactRole.Value > 0)
                    _Role = Configuration.Resolver.GetRepository<EntityContactRoleRepository>().GetById(this.IdEntityContactRole.Value);
                return _Role;
            }
        }
    }
}
