using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities
{
    [DataContract]
    public class ProductCategoryAttr : BaseEntity, ILinqExtent
    {
        [DataMember]
        public int IdProductCategory { get; set; }
        [DataMember(IsRequired = true)]
        public string IdAppLanguage { get; set; }
        [DataMember(IsRequired = true)]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Photo { get; set; }
        [DataMember(IsRequired = true)]
        public string Search { get; set; }

        private ProductCategory _ProductCategory = null;
        public ProductCategory ProductCategory
        {
            get
            {
                if (_ProductCategory == null && IdProductCategory > 0)
                    _ProductCategory = Configuration.Resolver.GetRepository<ProductCategoryRepository>().GetById(this.IdProductCategory);
                return _ProductCategory;
            }
        }
    }
}
