using Application.Migration.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Migration.Models
{
    public class Table
    {
        public Type Entity { get; set; }
        public string TableName { get; set; }
        public bool TableExists { get; set; }

        public Table(Type entity) : this(entity, Utilities.Converter.ToSnakeCase(entity.Name, '_'), false) { }
        public Table(Schema schema) : this(null, schema.TableName, true) { }
        public Table(Schema schema, Type entity) : this(entity, schema.TableName, true) { }
        public Table(Type entity, string tableName, bool tableExists)
        {
            Entity = entity;
            TableName = tableName;
            TableExists = tableExists;
        }

        private Activity? _operation;
        public Activity Operation
        {
            get
            {
                if (!_operation.HasValue)
                {
                    if (Entity == null && !TableExists)
                        _operation = Activity.Ignore;
                    else if (Entity != null && !TableExists)
                        _operation = Activity.Create;
                    else if (Entity == null && TableExists)
                        _operation = Activity.Drop;
                    else if (Entity != null && TableExists && TableColumns.Any(c => c.Operation != Activity.Ignore))
                        _operation = Activity.Update;
                    else
                        _operation = Activity.Ignore;
                }
                return _operation.Value;
            }
        }

        private Dictionary<string, Column> _Columns = new Dictionary<string, Column>();
        public Column this[string columnName]
        {
            get
            {
                if (!_Columns.ContainsKey(columnName))
                    return null;
                return _Columns[columnName];
            }
            set
            {
                value.Table = this;
                _Columns.Add(columnName, value);
            }
        }

        public List<Column> TableColumns
        {
            get
            {
                return _Columns.Values.ToList();
            }
        }

        public bool HasColumn(string columnName)
        {
            return _Columns.ContainsKey(columnName);
        }

        public bool RemoveColumn(string columnName)
        {
            return _Columns.Remove(columnName);
        }

        public TableSchema GetFullSchema()
        {
            var Schema = new TableSchema(TableName);
            TableColumns.ForEach(col => Schema.Columns.Add(col.GetSchema()));
            return Schema;
        }

        public TableSchema GetSchema(Activity activity)
        {
            var Schema = new TableSchema(TableName);
            foreach (var col in TableColumns.Where(t => t.Operation == activity))
            {
                Schema.Columns.Add(col.GetSchema());
            }
            return Schema;
        }

    }
}
