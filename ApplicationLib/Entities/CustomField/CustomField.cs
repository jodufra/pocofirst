using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;
using Application.Repositories;

namespace Application.Entities
{
    [DataContract]
    public class CustomField : BaseEntity, ILinqExtent, IEntityWithAttr
    {
        [DataMember]
        public int IdCustomFieldType { get; set; }
        [DataMember]
        public int? IdCustomFieldGroup { get; set; }
        [DataMember]
        public int? IdEntityType { get; set; }
        [DataMember]
        public int OrderId { get; set; }
        [DataMember(IsRequired = true)]
        public string Reference { get; set; }
        [DataMember]
        public string Default { get; set; }
        [DataMember]
        public bool IsVisibleInEntities { get; set; }
        [DataMember]
        public bool IsVersionAttribute { get; set; }
        [DataMember]
        public bool IsVisibleInProducts { get; set; }


        private CustomFieldType _Type = null;
        public CustomFieldType Type
        {
            get
            {
                if (_Type == null && IdCustomFieldType > 0)
                    _Type = Configuration.Resolver.GetRepository<CustomFieldTypeRepository>().GetById(this.IdCustomFieldType);
                return _Type;
            }
        }

        private CustomFieldGroup _Group = null;
        public CustomFieldGroup Group
        {
            get
            {
                if (_Group == null && IdCustomFieldGroup.HasValue && IdCustomFieldGroup.Value > 0)
                    _Group = Configuration.Resolver.GetRepository<CustomFieldGroupRepository>().GetById(this.IdCustomFieldGroup.Value);
                return _Group;
            }
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

        private string _Title = null;
        public string Title
        {
            get
            {
                if (string.IsNullOrEmpty(_Title) && !string.IsNullOrEmpty(Security.Session.IdAppLanguage))
                    _Title = this[Security.Session.IdAppLanguage].Title;
                return _Title;
            }
        }

        public bool HasLanguageDefined(string lang)
        {
            return Attrs.Where(s => s.IdAppLanguage == lang).FirstOrDefault() != null;
        }

        public IList<CustomFieldAttr> GetAttrs()
        {
            return Configuration.Resolver.GetRepository<CustomFieldRepository>().GetAttrs(this.Id);
        }

        private IList<CustomFieldAttr> _Attrs { get; set; }
        private IList<CustomFieldAttr> Attrs
        {
            get
            {
                if (_Attrs == null)
                    _Attrs = GetAttrs();
                return _Attrs;
            }
        }

        public Dictionary<string, CustomFieldAttr> _Attributes { get; set; }
        public CustomFieldAttr this[string IdAppLanguage]
        {
            get
            {
                if (_Attributes == null)
                    _Attributes = new Dictionary<string, CustomFieldAttr>();
                if (!_Attributes.ContainsKey(IdAppLanguage))
                {
                    var result = Attrs.Where(s => s.IdAppLanguage == IdAppLanguage).FirstOrDefault();
                    if (result == null)
                        result = new CustomFieldAttr() { IdAppLanguage = IdAppLanguage, IdCustomField = this.Id, DateCreated = DateTime.Now };
                    _Attributes.Add(IdAppLanguage, result);
                }
                return _Attributes[IdAppLanguage];
            }
        }
  
        public new IList<string> Populate(NameValueCollection data)
        {
            EntityExtensions.PopulateToProperties(this, data);
            var langs =  Configuration.Resolver.GetRepository<AppLanguageRepository>().GetAll();
            foreach (var language in langs)
            {
                var attr = this[language.Id];
                attr.PopulateToPropertiesByLanguage(data, language.Id);
            }
            return new List<string>();
        }

        public Boolean IsMultilanguage
        {
            get
            {
                return IdCustomFieldType == 2 || IdCustomFieldType == 3;
            }
        }

        private IList<CustomFieldOption> _Options { get; set; }
        public IList<CustomFieldOption> Options
        {
            get
            {
                if (_Options == null)
                    _Options =  Configuration.Resolver.GetRepository<CustomFieldOptionRepository>().GetByCustomFieldId(this.Id);
                return _Options;
            }
        }

        public string GetOptionValue(int index, string lang)
        {
            var item = this.Options.Where(s => s.Index == index).FirstOrDefault();
            return item != null ? item[lang].Value : null;
        }

        public bool TryReadValue(string str, out string result)
        {
            result = str;
            switch (IdCustomFieldType)
            {
                case CustomFieldType.TEXT:
                case CustomFieldType.SELECTION:
                case CustomFieldType.SELECTION_MULTIPLE:
                case CustomFieldType.TEXT_HTML:
                    return true;
                case CustomFieldType.INTEGER:
                    int temp;
                    return Int32.TryParse(str, out temp);
                case CustomFieldType.DATE:
                    try
                    {
                        return Configuration.Converter.ToDate(str).HasValue;
                    }
                    catch { return false; }
                case CustomFieldType.DECIMAL:
                case CustomFieldType.CURRENCY:
                    try
                    {
                       
                        return true;
                    }
                    catch { return false; }
                case CustomFieldType.BOOLEAN:
                    result = !string.IsNullOrEmpty(str) ? "1" : "0";
                    return true;
            }
            return false;
        }
    }
}
