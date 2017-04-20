using System.Runtime.Serialization;

namespace Application.Migration.Entities
{
    [DataContract]
    public class Schema
    {
        [DataMember]
        public string TableName { get; set; }
        [DataMember]
        public string ColumnName { get; set; }
        [DataMember]
        public string DataType { get; set; }
        [DataMember]
        public bool IsNullable { get; set; }
    }
}
