using Application.Migration.Entities;
using Application.Migration.Utilities;
using System;
using System.Reflection;

namespace Application.Migration.Models
{
    public class Column
    {
        public Table Table { get; set; }
        public PropertyInfo Field { get; set; }
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
        public bool ColumnNullable { get; set; }
        public bool ColumnExists { get; set; }

        public Column(Schema schema) : this(null, schema.ColumnName, schema.DataType, schema.IsNullable, true) { }
        public Column(PropertyInfo field) : this(field, field.Name, Converter.ToDBType(field.PropertyType), field.IsNullable(), false) { }
        public Column(PropertyInfo field, Schema schema) : this(field, schema.ColumnName, schema.DataType, schema.IsNullable, true) { }
        public Column(PropertyInfo field, string columnName, string columnType, bool columnNullable, bool columnExists)
        {
            Field = field;
            ColumnName = columnName;
            ColumnType = columnType;
            ColumnNullable = columnNullable;
            ColumnExists = columnExists;
        }

        public string GetParsedFieldType()
        {
            if (Field == null) return "";
            return Converter.ToDBType(Field.PropertyType);
        }

        public bool HasDifferentType()
        {
            if (Field == null) return false;
            return ColumnType != Converter.ToDBType(Field.PropertyType);
        }

        public bool HasDifferentRestriction()
        {
            if (Field == null) return false;
            return ColumnNullable != Field.IsNullable();
        }

        private Activity? _operation;
        public Activity Operation
        {
            get
            {
                if (!_operation.HasValue)
                {
                    if (Field == null && !ColumnExists)
                        _operation = Activity.Ignore;
                    if (Field != null && !ColumnExists)
                        _operation = Activity.Create;
                    else if (Field == null && ColumnExists)
                        _operation = Activity.Drop;
                    else if (Field != null && ColumnExists)
                        if (HasDifferentType() || HasDifferentRestriction())
                            _operation = Activity.Update;
                        else
                            _operation = Activity.Ignore;
                    else
                        _operation = Activity.Ignore;
                }
                return _operation.Value;
            }
        }


        private ColumnSchema Schema = null;
        public ColumnSchema GetSchema()
        {
            if (Schema == null)
            {
                Schema = new ColumnSchema(Table.TableName, ColumnName, Converter.ToDBLengthType(Field.PropertyType), Field.IsNullable());
            }
            return Schema;
        }

    }
}
