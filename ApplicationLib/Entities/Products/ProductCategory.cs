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
    public class ProductCategory : BaseEntity, ILinqExtent, IEntityWithAttr
    {

        [DataMember]
        public int? IdParent { get; set; }
        [DataMember(IsRequired = true)]
        public string Path { get; set; }
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

        private ProductCategory _Parent = null;
        public ProductCategory Parent
        {
            get
            {
                if (_Parent == null && IdParent.HasValue && IdParent.Value > 0)
                    _Parent = Configuration.Resolver.GetRepository<ProductCategoryRepository>().GetById(this.IdParent.Value);
                return _Parent;
            }
        }

        private IList<ProductCategory> _Childs { get; set; }
        public IList<ProductCategory> Childs
        {
            get
            {
                if (_Childs == null)
                    _Childs = Configuration.Resolver.GetRepository<ProductCategoryRepository>().GetByParent(this.Id);
                return _Childs;
            }
        }

        private IList<ProductCategoryAttr> _Attrs { get; set; }
        public IList<ProductCategoryAttr> Attrs
        {
            get
            {
                if (_Attrs == null)
                    _Attrs = this.GetAttrs();
                return _Attrs;
            }
        }

        public Dictionary<string, ProductCategoryAttr> _Attributes { get; set; }
        public ProductCategoryAttr this[string IdAppLanguage]
        {
            get
            {
                if (_Attributes == null)
                    _Attributes = new Dictionary<string, ProductCategoryAttr>();
                if (!_Attributes.ContainsKey(IdAppLanguage))
                {
                    var result = Attrs.Where(s => s.IdAppLanguage == IdAppLanguage).FirstOrDefault();
                    if (result == null)
                        result = new ProductCategoryAttr() { IdAppLanguage = IdAppLanguage, IdProductCategory = this.Id, DateCreated = DateTime.Now };
                    _Attributes.Add(IdAppLanguage, result);
                }
                return _Attributes[IdAppLanguage];
            }
        }

        public IList<ProductCategoryAttr> GetAttrs()
        {
            return Configuration.Resolver.GetRepository<ProductCategoryRepository>().GetAttrs(this.Id);
        }

        public bool HasLanguageDefined(string lang)
        {
            return Attrs.Where(s => s.IdAppLanguage == lang).FirstOrDefault() != null;
        }

        public string GetDirectoryPhotosSrc()
        {
            return "/" + System.Configuration.ConfigurationManager.AppSettings["Folders:Products:Categories"].Replace("{id}", this.Id.ToString());
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
