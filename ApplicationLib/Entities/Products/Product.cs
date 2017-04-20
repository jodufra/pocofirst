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
    public class Product : BaseEntity, ILinqExtent, IEntityWithAttr
    {
        [DataMember]
        public int IdAppTax { get; set; }
        [DataMember]
        public int? IdProductBrand { get; set; }
        [DataMember(IsRequired = true)]
        public string Reference { get; set; }
        [DataMember]
        public bool HasReviews { get; set; }
        [DataMember]
        public bool IsHighlight { get; set; }
        [DataMember]
        public DateTime? DatePublished { get; set; }

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

        public string PhotoSrc
        {
            get
            {
                return this.Photos.Count > 0 ? this.Photos[0].GetPhotoSrc() : null;
            }
        }

        private AppTax _ProductTax = null;
        public AppTax ProductTax
        {
            get
            {
                if (_ProductTax == null && IdAppTax > 0)
                    _ProductTax = Configuration.Resolver.GetRepository<AppTaxRepository>().GetById(this.IdAppTax);
                return _ProductTax;
            }
        }

        private ProductBrand _ProductBrand = null;
        public ProductBrand ProductBrand
        {
            get
            {
                if (_ProductBrand == null && IdProductBrand.HasValue && IdProductBrand > 0)
                    _ProductBrand = Configuration.Resolver.GetRepository<ProductBrandRepository>().GetById(this.IdProductBrand.Value);
                return _ProductBrand;
            }
        }

        private IList<ProductPhoto> _Photos = null;
        public IList<ProductPhoto> Photos
        {
            get
            {
                if (_Photos == null)
                    _Photos = Configuration.Resolver.GetRepository<ProductPhotoRepository>().GetByProduct(this.Id);
                return _Photos;
            }
        }

        public IList<ProductAttr> GetAttrs()
        {
            return Configuration.Resolver.GetRepository<ProductRepository>().GetAttrs(this.Id);
        }

        public bool HasLanguageDefined(string lang)
        {
            return Attrs.Where(s => s.IdAppLanguage == lang).FirstOrDefault() != null;
        }

        private IList<ProductAttr> _Attrs { get; set; }
        public IList<ProductAttr> Attrs
        {
            get
            {
                if (_Attrs == null)
                    _Attrs =  this.GetAttrs();
                return _Attrs;
            }
        }

        public Dictionary<string, ProductAttr> _Attributes { get; set; }
        public ProductAttr this[string IdAppLanguage]
        {
            get
            {
                if (_Attributes == null)
                    _Attributes = new Dictionary<string, ProductAttr>();
                if (!_Attributes.ContainsKey(IdAppLanguage))
                {
                    var result = Attrs.Where(s => s.IdAppLanguage == IdAppLanguage).FirstOrDefault();
                    if (result == null)
                        result = new ProductAttr() { IdAppLanguage = IdAppLanguage, IdProduct = this.Id, DateCreated = DateTime.Now };
                    _Attributes.Add(IdAppLanguage, result);
                }
                return _Attributes[IdAppLanguage];
            }
        }
        
        private IList<ProductVsCustomField> _Fields { get; set; }
        public IList<ProductVsCustomField> Fields
        {
            get
            {
                if (_Fields == null)
                    _Fields =  Configuration.Resolver.GetRepository<ProductVsCustomFieldRepository>().GetByProduct(this.Id);
                return _Fields;
            }
        }

        private IList<CustomField> _AvailableFields { get; set; }
        public IList<CustomField> AvailableFields
        {
            get
            {
                if (_AvailableFields == null)
                    _AvailableFields =  Configuration.Resolver.GetRepository<CustomFieldRepository>().GetByProducts();
                return _AvailableFields;
            }
        }

        public IList<Icon> _Icons = null;
        public IList<Icon> Icons
        {
            get
            {
                if (_Icons == null)
                {
                    _Icons = Configuration.Resolver.GetRepository<ProductVsIconRepository>().GetIconsByProduct(this.Id);
                }
                return _Icons;
            }
        }

        public string GetFieldValue(string reference, string lang)
        {
            var result = Fields.Where(s => s.Reference == reference && s.IdAppLanguage == lang).FirstOrDefault();
            return result != null ? result.Value : null;
        }

        public Dictionary<string, ProductVsCustomField> _CustomFields { get; set; }
        public ProductVsCustomField this[string Reference, string Language]
        {
            get
            {
                var key = (Reference + "-" + Language).ToLower();
                if (_CustomFields == null)
                    _CustomFields = new Dictionary<string, ProductVsCustomField>();
                if (!_CustomFields.ContainsKey(key))
                {
                    var field = AvailableFields.Where(s => s.Reference == Reference).FirstOrDefault();
                    if (field == null)
                        throw new Exception("Field not available");
                    var result = Fields.Where(s => s.Reference == Reference && ((field.IsMultilanguage && s.IdAppLanguage == Language) || !field.IsMultilanguage)).FirstOrDefault();
                    if (result == null)
                    {
                        result = new ProductVsCustomField() { IdProduct = this.Id, DateCreated = DateTime.Now, Reference = Reference, Value = "", IdCustomField = field.Id };
                        if (field.IsMultilanguage)
                            result.IdAppLanguage = Language;
                    }
                    _CustomFields.Add(key, result);
                }
                return _CustomFields[key];
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

        public string GetDirectoryFilesSrc()
        {
            return "/" + System.Configuration.ConfigurationManager.AppSettings["Folders:Products:Files"].Replace("{id}", this.Id.ToString());
        }

        public string GetDirectoryPhotosSrc()
        {
            return "/" + System.Configuration.ConfigurationManager.AppSettings["Folders:Products:Photos"].Replace("{id}", this.Id.ToString());
        }

    }
}
