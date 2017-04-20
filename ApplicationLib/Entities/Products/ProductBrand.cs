using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities
{
    [DataContract]
    public class ProductBrand : BaseEntity, ILinqExtent, IEntityWithAttr
    {
        [DataMember]
        public int OrderId { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool IsHighlight { get; set; }

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

        public IList<ProductBrandAttr> GetAttrs()
        {
            return Configuration.Resolver.GetRepository<ProductBrandRepository>().GetAttrs(this.Id);
        }

        public bool HasLanguageDefined(string lang)
        {
            return Attrs.Where(s => s.IdAppLanguage == lang).FirstOrDefault() != null;
        }

        private IList<ProductBrandAttr> _Attrs { get; set; }
        public IList<ProductBrandAttr> Attrs
        {
            get
            {
                if (_Attrs == null)
                    _Attrs = this.GetAttrs();
                return _Attrs;
            }
        }

        public Dictionary<string, ProductBrandAttr> _Attributes { get; set; }
        public ProductBrandAttr this[string IdAppLanguage]
        {
            get
            {
                if (_Attributes == null)
                    _Attributes = new Dictionary<string, ProductBrandAttr>();
                if (!_Attributes.ContainsKey(IdAppLanguage))
                {
                    var result = Attrs.Where(s => s.IdAppLanguage == IdAppLanguage).FirstOrDefault();
                    if (result == null)
                        result = new ProductBrandAttr() { IdAppLanguage = IdAppLanguage, IdProductBrand = this.Id, DateCreated = DateTime.Now };
                    _Attributes.Add(IdAppLanguage, result);
                }
                return _Attributes[IdAppLanguage];
            }
        }

        public new IList<string> Populate(NameValueCollection data)
        {
            EntityExtensions.PopulateToProperties(this, data);
            var langs = Configuration.Resolver.GetRepository<AppLanguageRepository>().GetAll();
            foreach (var language in langs)
            {
                var attr = this[language.Id];
                attr.PopulateToPropertiesByLanguage(data, language.Id);
            }
            return new List<string>();
        }

        public string GetDirectoryPhotosSrc()
        {
            return "/" + System.Configuration.ConfigurationManager.AppSettings["Folders:Products:Brands"].Replace("{id}", this.Id.ToString());
        }
    }
}
