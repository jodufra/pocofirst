using System;
using System.Globalization;

namespace Application.Migration.Utilities
{
    public static class Converter
    {

        public static Type ConvertToCsharpType(string type, bool isNullable)
        {
            switch (type)
            {
                case "varchar":
                    return typeof(string);
                case "datetime":
                    return isNullable ? typeof(DateTime?) : typeof(DateTime);
                case "int":
                    return isNullable ? typeof(int?) : typeof(int);
                case "smallint":
                    return isNullable ? typeof(short?) : typeof(short);
                case "decimal":
                    return isNullable ? typeof(decimal?) : typeof(decimal);
                case "numeric":
                    return isNullable ? typeof(decimal?) : typeof(decimal);
                case "money":
                    return isNullable ? typeof(decimal?) : typeof(decimal);
                case "bigint":
                    return isNullable ? typeof(long?) : typeof(long);
                case "tinyint":
                    return isNullable ? typeof(byte?) : typeof(byte);
                case "char":
                    return typeof(string);
                case "timestamp":
                    return typeof(byte[]);
                case "varbinary":
                    return typeof(byte[]);
                case "bit":
                    return isNullable ? typeof(bool?) : typeof(bool);
                case "xml":
                    return typeof(string);
            }
            return null;
        }

        public static string ToDBType(Type type)
        {
            if (type == null) return "";
            if (type.IsNullable())
                type = type.GetGenericArguments()[0];
            switch (type.Name)
            {
                case "string":
                case "String":
                    return "varchar";
                case "DateTime":
                    return "datetime";
                case "int":
                case "Int32":
                    return "int";
                case "short":
                case "Int16":
                    return "smallint";
                case "decimal":
                case "Decimal":
                    return "decimal";
                case "long":
                    return "bigint";
                case "byte":
                    return "tinyint";
                case "byte[]":
                    return "varbinary";
                case "bool":
                case "Boolean":
                    return "bit";
            }
            return "";
        }

        public static string ToDBLengthType(Type type)
        {
            if (type == null) return "";
            if (type.IsNullable())
                type = type.GetGenericArguments()[0];
            switch (type.Name)
            {
                case "string":
                case "String":
                    return "varchar(255)";
                case "DateTime":
                    return "datetime";
                case "int":
                case "Int32":
                    return "int";
                case "short":
                case "Int16":
                    return "smallint";
                case "decimal":
                case "Decimal":
                    return "decimal";
                case "long":
                    return "bigint";
                case "byte":
                    return "tinyint";
                case "byte[]":
                    return "varbinary(255)";
                case "bool":
                case "Boolean":
                    return "bit(1)";
            }
            return "";
        }

        public static string ToSnakeCase(this string propertyName, char separator)
        {
            var sep = separator.ToString();
            for (int j = propertyName.Length - 1; j > 0; j--)
                if (j > 0 && char.IsUpper(propertyName[j]))
                    propertyName = propertyName.Insert(j, sep);
            return propertyName.ToLower();
        }

        public static string ToPascalCase(this string propertyName, char separator)
        {
            propertyName = propertyName.Replace(separator, ' ');
            TextInfo ti = new CultureInfo("en-US", false).TextInfo;
            return ti.ToTitleCase(propertyName).Replace(" ", string.Empty);
        }

    }
}
