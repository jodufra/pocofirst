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
    public class CustomFieldOption : BaseEntity, ILinqExtent, IEntityWithAttr
    {
        [DataMember]
        public int IdCustomField { get; set; }
        [DataMember]
        public int Index { get; set; }

        private CustomField _CustomField = null;
        public CustomField CustomField
        {
            get
            {
                if (_CustomField == null && IdCustomField > 0)
                    _CustomField =  Configuration.Resolver.GetRepository<CustomFieldRepository>().GetById(this.IdCustomField);
                return _CustomField;
            }
        }

        public bool HasLanguageDefined(string lang)
        {
            return Attrs.Where(s => s.IdAppLanguage == lang).FirstOrDefault() != null;
        }

        public IList<CustomFieldOptionAttr> GetAttrs()
        {
            return Configuration.Resolver.GetRepository<CustomFieldOptionRepository>().GetAttrs(this.Id);
        }

        private IList<CustomFieldOptionAttr> _Attrs { get; set; }
        private IList<CustomFieldOptionAttr> Attrs
        {
            get
            {
                if (_Attrs == null)
                    _Attrs = GetAttrs();
                return _Attrs;
            }
        }

        public Dictionary<string, CustomFieldOptionAttr> _Attributes { get; set; }
        public CustomFieldOptionAttr this[string IdAppLanguage]
        {
            get
            {
                if (_Attributes == null)
                    _Attributes = new Dictionary<string, CustomFieldOptionAttr>();
                if (!_Attributes.ContainsKey(IdAppLanguage))
                {
                    var result = Attrs.Where(s => s.IdAppLanguage == IdAppLanguage).FirstOrDefault();
                    if (result == null)
                        result = new CustomFieldOptionAttr() { IdAppLanguage = IdAppLanguage, IdCustomFieldOption = this.Id, DateCreated = DateTime.Now };
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
    }
}
