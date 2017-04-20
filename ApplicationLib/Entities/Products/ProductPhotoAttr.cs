using System.Linq;
using System.Runtime.Serialization;
using Application.Repositories;
using System;

namespace Application.Entities
{
    [DataContract]
    public class ProductPhotoAttr : BaseEntity, ILinqExtent
    {
        [DataMember]
        public int IdProductPhoto { get; set; }
        [DataMember(IsRequired = true)]
        public string IdAppLanguage { get; set; }
        [DataMember(IsRequired = true)]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }

        private ProductPhoto _ProductPhoto = null;
        public ProductPhoto ProductPhoto
        {
            get
            {
                if (_ProductPhoto == null && IdProductPhoto > 0)
                    _ProductPhoto = Configuration.Resolver.GetRepository<ProductPhotoRepository>().GetById(this.IdProductPhoto);
                return _ProductPhoto;
            }
        }
    }
}
