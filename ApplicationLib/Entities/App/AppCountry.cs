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
    public class AppCountry : ILinqExtent, IEntityWithAttr
    {
        [DataMember(IsRequired = true)]
        public string Id { get; set; }
        [DataMember]
        public bool IncludeTax { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public DateTime? DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }

        private string _Name = null;
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_Name) && !string.IsNullOrEmpty(Security.Session.IdAppLanguage))
                    _Name = this[Security.Session.IdAppLanguage].Name;
                return _Name;
            }
        }

        public bool HasLanguageDefined(string lang)
        {
            return Attrs.Where(s => s.IdAppLanguage == lang).FirstOrDefault() != null;
        }

        public IList<AppCountryAttr> GetAttrs()
        {
            return Configuration.Resolver.GetRepository<AppCountryRepository>().GetAttrs(this.Id);
        }

        private IList<AppCountryAttr> _Attrs { get; set; }
        private IList<AppCountryAttr> Attrs
        {
            get
            {
                if (_Attrs == null)
                    _Attrs = GetAttrs();
                return _Attrs;
            }
        }

        public Dictionary<string, AppCountryAttr> _Attributes { get; set; }
        public AppCountryAttr this[string IdAppLanguage]
        {
            get
            {
                if (_Attributes == null)
                    _Attributes = new Dictionary<string, AppCountryAttr>();
                if (!_Attributes.ContainsKey(IdAppLanguage))
                {
                    var result = Attrs.Where(s => s.IdAppLanguage == IdAppLanguage).FirstOrDefault();
                    if (result == null)
                        result = new AppCountryAttr() { IdAppLanguage = IdAppLanguage, IdAppCountry = this.Id, DateCreated = DateTime.Now };
                    _Attributes.Add(IdAppLanguage, result);
                }
                return _Attributes[IdAppLanguage];
            }
        }

        public IList<string> Populate(NameValueCollection data)
        {
            this.Id = data["Id"].ToLower();
            this.IsActive = !string.IsNullOrEmpty(data["IsActive"]);
            this.IncludeTax = !string.IsNullOrEmpty(data["IncludeTax"]);
            var langs = Configuration.Resolver.GetRepository<AppLanguageRepository>().GetAll();
            foreach (var language in langs)
            {
                var attr = this[language.Id];
                attr.PopulateToPropertiesByLanguage(data, language.Id);
            }
            return new List<string>();
        }

        public bool IsNew()
        {
            return !DateCreated.HasValue;
        }
    }
}
