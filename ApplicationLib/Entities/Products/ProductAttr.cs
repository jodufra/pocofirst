using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;
using Application.Repositories;
using System;

namespace Application.Entities
{
    [DataContract]
    public class ProductAttr : BaseEntity, ILinqExtent
    {
        [DataMember]
        public int IdProduct { get; set; }
        [DataMember(IsRequired = true)]
        public string IdAppLanguage { get; set; }
        [DataMember(IsRequired = true)]
        public string Title { get; set; }
        [DataMember]
        public string Introduction { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SEOKeywords { get; set; }
        [DataMember]
        public string SEODescription { get; set; }
        [DataMember(IsRequired = true)]
        public string Search { get; set; }

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
    }
}
