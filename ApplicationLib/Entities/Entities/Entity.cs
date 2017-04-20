using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;
using Application.Repositories;

namespace Application.Entities
{
    [DataContract]
    public class Entity : BaseEntity, ILinqExtent
    {
        [DataMember]
        public int? IdEntityType { get; set; }
        [DataMember]
        public int? IdEntityGroup { get; set; }
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
        [DataMember]
        public string FiscalId { get; set; }
        [DataMember]
        public string Photo { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string ContactDefault { get; set; }
        [DataMember]
        public string ContactOptional { get; set; }
        [DataMember]
        public string Website { get; set; }
        [DataMember]
        public string Notes { get; set; }
        [DataMember(IsRequired = true)]
        public string Search { get; set; }
        
        public string PhotoSrc
        {
            get
            {
                return !string.IsNullOrEmpty(this.Photo) ? GetPhotoSrc() : null;
            }
        }

        public string GetPhotoSrc()
        {
            return string.IsNullOrEmpty(this.Photo) ? "#" : "/" + System.Configuration.ConfigurationManager.AppSettings["Folders:Entities:Photos"] + "/" + this.Photo;
        }

        private EntityType _EntityType = null;
        public EntityType EntityType
        {
            get
            {
                if (_EntityType == null && IdEntityType.HasValue && IdEntityType.Value > 0)
                    _EntityType = Configuration.Resolver.GetRepository<EntityTypeRepository>().GetById(this.IdEntityType.Value);
                return _EntityType;
            }
        }
        private EntityGroup _EntityGroup = null;
        public EntityGroup EntityGroup
        {
            get
            {
                if (_EntityGroup == null && IdEntityGroup.HasValue && IdEntityGroup.Value > 0)
                    _EntityGroup = Configuration.Resolver.GetRepository<EntityGroupRepository>().GetById(this.IdEntityGroup.Value);
                return _EntityGroup;
            }
        }

        private IList<EntityAddress> _Addresses = null;
        public IList<EntityAddress> Addresses
        {
            get
            {
                if (_Addresses == null)
                    _Addresses =  Configuration.Resolver.GetRepository<EntityAddressRepository>().GetByEntity(this.Id);
                return _Addresses;
            }
        }

        private IList<EntityContact> _Contacts = null;
        public IList<EntityContact> Contacts
        {
            get
            {
                if (_Contacts == null)
                    _Contacts =  Configuration.Resolver.GetRepository<EntityContactRepository>().GetByEntity(this.Id);
                return _Contacts;
            }
        }

        private IList<EntityVsCustomField> _Fields { get; set; }
        private IList<EntityVsCustomField> Fields
        {
            get
            {
                if (_Fields == null)
                    _Fields =  Configuration.Resolver.GetRepository<EntityVsCustomFieldRepository>().GetByEntity(this.Id);
                return _Fields;
            }
        }

        private IList<CustomField> _AvailableFields { get; set; }
        private IList<CustomField> AvailableFields
        {
            get
            {
                if (_AvailableFields == null)
                    _AvailableFields =  Configuration.Resolver.GetRepository<CustomFieldRepository>().GetByEntities();
                return _AvailableFields;
            }
        }

        public string GetFieldValue(string reference, string lang)
        {
            var result = Fields.Where(s => s.Reference == reference && s.IdAppLanguage == lang).FirstOrDefault();
            return result != null ? result.Value : null;
        }

        public Dictionary<string, EntityVsCustomField> _CustomFields { get; set; }
        public EntityVsCustomField this[string Reference, string Language]
        {
            get
            {
                var key = (Reference + "-" + Language).ToLower();
                if (_CustomFields == null)
                    _CustomFields = new Dictionary<string, EntityVsCustomField>();
                if (!_CustomFields.ContainsKey(key))
                {
                    var field = AvailableFields.Where(s => s.Reference == Reference).FirstOrDefault();
                    if (field == null)
                        throw new Exception("Field not available");
                    var result = Fields.Where(s => s.Reference == Reference && ((field.IsMultilanguage && s.IdAppLanguage == Language) || !field.IsMultilanguage)).FirstOrDefault();
                    if (result == null)
                    {
                        result = new EntityVsCustomField() { IdEntity = this.Id, DateCreated = DateTime.Now, Reference = Reference, Value = "", IdCustomField = field.Id };
                        if (field.IsMultilanguage)
                            result.IdAppLanguage = Language;
                    }
                    _CustomFields.Add(key, result);
                }
                return _CustomFields[key];
            }
        }
    }
}
