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
    public class ProductBrandAttr : BaseEntity, ILinqExtent
    {
        [DataMember]
        public int IdProductBrand { get; set; }
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

        private ProductBrand _ProductBrand = null;
        public ProductBrand ProductBrand
        {
            get
            {
                if (_ProductBrand == null && IdProductBrand > 0)
                    _ProductBrand = Configuration.Resolver.GetRepository<ProductBrandRepository>().GetById(this.IdProductBrand);
                return _ProductBrand;
            }
        }

        public string GetPhotoSrc()
        {
            return string.IsNullOrEmpty(this.Photo) ? "#" : "/" + System.Configuration.ConfigurationManager.AppSettings["Folders:Products:Brands"] + "/" + this.Photo;
        }
    }
}
