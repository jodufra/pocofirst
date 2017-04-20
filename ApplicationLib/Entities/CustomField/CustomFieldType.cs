using System.Linq;
using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class CustomFieldType : BaseEntity, ILinqExtent
    {
        public const int BOOLEAN = 1;
        public const int TEXT = 2;
        public const int TEXT_HTML = 3;
        public const int DATE = 4;
        public const int TIME = 5;
        public const int INTEGER = 6;
        public const int DECIMAL = 7;
        public const int CURRENCY = 8;
        public const int SELECTION = 9;
        public const int SELECTION_MULTIPLE = 10;

        [DataMember(IsRequired = true)]
        public string Title { get; set; }
    }
}
