using System.Collections.Generic;
using System.Linq;

namespace Application.Migration.Models
{
    public class TableSchema
    {
        public string Name { get; set; }
        public List<ColumnSchema> Columns { get; set; }

        public TableSchema(string name)
        {
            Name = name;
            Columns = new List<ColumnSchema>();
        }

        public List<ColumnSchema> GetPrimaryKeys()
        {
            var keys = Columns.Where(c => c.IsPrimaryKey()).ToList();
            if (!keys.Any() && Name.Contains("_vs_"))
                return GetForeignKeys();
            return keys;

        }

        public List<ColumnSchema> GetForeignKeys()
        {
            return Columns.Where(c => c.IsForeignKey()).ToList();
        }

    }
}
