using Application.Migration.Utilities;

namespace Application.Migration.Models
{
    public class ColumnSchema
    {
        public string TableName { get; set; }
        public string Name { get; set; }
        public string NameSnakeCase { get; set; }
        public string Type { get; set; }
        public bool Nullable { get; set; }
        public int Order
        {
            get
            {
                return IsPrimaryKey() ? 0 :
                    IsForeignKey() ? 1 :
                    NameSnakeCase.StartsWith("name") || NameSnakeCase.StartsWith("title") ? 2 :
                    NameSnakeCase.StartsWith("is_") ? 9 :
                    NameSnakeCase.StartsWith("date_") ? 10 : 8;
            }
        }

        public ColumnSchema(string tableName, string name, string type, bool nullable)
        {
            TableName = tableName;
            Name = name;
            NameSnakeCase = Name.ToSnakeCase('_');
            Type = type;
            Nullable = nullable;

            _IsPrimaryKey = Name == "Id";
            _IsForeignKey = NameSnakeCase.StartsWith("id_");
            _ForeignKeyReference = _IsForeignKey ? (Name == "IdParent" ? tableName.ToSnakeCase('_') : NameSnakeCase.Substring(3)) : "";
            _AutoIncrementable = !Type.Contains("varchar") && !Type.Contains("datetime") && !Type.Contains("varbinary") && !Type.Contains("bit");
        }

        private bool _IsPrimaryKey;
        public bool IsPrimaryKey() { return _IsPrimaryKey; }

        private bool _IsForeignKey;
        public bool IsForeignKey() { return _IsForeignKey; }

        private string _ForeignKeyReference;
        public string ForeignKeyReference() { return _ForeignKeyReference; }

        private bool _AutoIncrementable;
        public bool AutoIncrementable() { return _AutoIncrementable; }

    }
}
