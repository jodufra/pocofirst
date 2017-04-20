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
    public class ProductPhoto : BaseEntity, ILinqExtent, IEntityWithAttr
    {
        [DataMember]
        public int IdProduct { get; set; }
        [DataMember]
        public int OrderId { get; set; }
        [DataMember(IsRequired = true)]
        public string Filename { get; set; }

        public string GetPhotoSrc()
        {
            return string.IsNullOrEmpty(this.Filename) ? "#" : "/" + System.Configuration.ConfigurationManager.AppSettings["Folders:Products:Photos"].Replace("{id}",this.IdProduct.ToString()) + "/" + this.Filename;
        }

        private Product _Product = null;
        public Product Product
        {
            get
            {
                if (_Product == null && IdProduct > 0)
                    _Product = Configuration.Resolver.GetRepository<ProductRepository>().GetById(this.IdProduct);
                return _Product;
            }
        }

        public bool HasLanguageDefined(string lang)
        {
            return Attrs.Where(s => s.IdAppLanguage == lang).FirstOrDefault() != null;
        }

        public IList<ProductPhotoAttr> GetAttrs()
        {
            return Configuration.Resolver.GetRepository<ProductPhotoRepository>().GetAttrs(this.Id);
        }

        private IList<ProductPhotoAttr> _Attrs { get; set; }
        private IList<ProductPhotoAttr> Attrs
        {
            get
            {
                if (_Attrs == null)
                    _Attrs = GetAttrs();
                return _Attrs;
            }
        }

        public Dictionary<string, ProductPhotoAttr> _Attributes { get; set; }
        public ProductPhotoAttr this[string IdAppLanguage]
        {
            get
            {
                if (_Attributes == null)
                    _Attributes = new Dictionary<string, ProductPhotoAttr>();
                if (!_Attributes.ContainsKey(IdAppLanguage))
                {
                    var result = Attrs.Where(s => s.IdAppLanguage == IdAppLanguage).FirstOrDefault();
                    if (result == null)
                        result = new ProductPhotoAttr() { IdAppLanguage = IdAppLanguage, IdProductPhoto = this.Id, DateCreated = DateTime.Now };
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
    }
}
