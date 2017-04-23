using System.Runtime.Serialization;

namespace Application.Entities
{
    [DataContract]
    public class AppLanguage
    {
        [DataMember(IsRequired = true)]
        public string Id { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }

    }
}
