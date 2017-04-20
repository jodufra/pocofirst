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
    public class CustomFieldGroup : BaseEntity, ILinqExtent, IEntityWithAttr
    {
        [DataMember]
        public int OrderId { get; set; }

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

        public IList<CustomFieldGroupAttr> GetAttrs()
        {
            return Configuration.Resolver.GetRepository<CustomFieldGroupRepository>().GetAttrs(this.Id);
        }

        private IList<CustomFieldGroupAttr> _Attrs { get; set; }
        private IList<CustomFieldGroupAttr> Attrs
        {
            get
            {
                if (_Attrs == null)
                    _Attrs = GetAttrs();
                return _Attrs;
            }
        }

        public Dictionary<string, CustomFieldGroupAttr> _Attributes { get; set; }
        public CustomFieldGroupAttr this[string IdAppLanguage]
        {
            get
            {
                if (_Attributes == null)
                    _Attributes = new Dictionary<string, CustomFieldGroupAttr>();
                if (!_Attributes.ContainsKey(IdAppLanguage))
                {
                    var result = Attrs.Where(s => s.IdAppLanguage == IdAppLanguage).FirstOrDefault();
                    if (result == null)
                        result = new CustomFieldGroupAttr() { IdAppLanguage = IdAppLanguage, IdCustomFieldGroup = this.Id, DateCreated = DateTime.Now };
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
