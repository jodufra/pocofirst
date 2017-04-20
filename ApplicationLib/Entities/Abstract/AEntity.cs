using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Reflection;
using System.Collections;

namespace Application.Entities
{
    public class EntityPropConfiguration
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public Object Value { get; set; }

        public string NameToSql { get { return "`" + Name + "`"; } }
        public string ValueToSql
        {
            get
            {
                if (Value == null)
                    return null;
                if (Type == typeof(int))
                    return GetInteger((int)Value);
                if (Type == typeof(int?))
                    return GetInteger((int?)Value);
                if (Type == typeof(Boolean))
                    return GetBoolean((Boolean)Value);
                if (Type == typeof(Boolean?))
                    return GetBoolean((Boolean?)Value);
                if (Type == typeof(DateTime))
                    return GetDate((DateTime)Value);
                if (Type == typeof(DateTime?))
                    return GetDate((DateTime?)Value);
                if (Type == typeof(decimal))
                    return GetNumber((decimal)Value);
                if (Type == typeof(decimal?))
                    return GetNumber((decimal?)Value);
                 return "'" + Value.ToString() + "'";
            }
        }

        private string GetDate(DateTime? v)
        {
            return !v.HasValue ? null : "'" + v.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'";
        }

        private string GetInteger(int? v)
        {
            return !v.HasValue ? null : v.ToString();
        }

        private string GetNumber(decimal? v)
        {
            return !v.HasValue ? null : v.Value.ToString().Replace(",",".");
        }

        private string GetBoolean(bool? v)
        {
            return !v.HasValue ? null : ((bool) v) ? "1" : "0";
        }
    }
    public abstract class AEntity : IEntity
    {
        public static List<string> PrivateFields = new List<string>() { "Id", "DateCreated", "DateUpdated" };

        public abstract int Id { get; set; }
        public abstract DateTime DateCreated { get; set; }
        public abstract DateTime? DateUpdated { get; set; }

        public static List<EntityPropConfiguration> GetProperties(object item)
        {
            var result = new List<EntityPropConfiguration>();
            var type = item.GetType();
            foreach (var prop in type.GetProperties().Where(p => Attribute.IsDefined(p, typeof(DataMemberAttribute))))
                result.Add( new EntityPropConfiguration() { Name = prop.Name, Value = prop.GetValue(item), Type = prop.PropertyType });
            return result;
        }

        public static List<EntityPropConfiguration> GetPropertiesWithSetter(object item)
        {
            var result = new List<EntityPropConfiguration>();
            var type = item.GetType();
            foreach (var prop in type.GetProperties().Where(p => p.CanWrite && Attribute.IsDefined(p, typeof(DataMemberAttribute))))
                result.Add(new EntityPropConfiguration() { Name = prop.Name, Value = prop.GetValue(item), Type = prop.PropertyType });
            return result;
        }

        public bool IsNew()
        {
            return this.Id == 0;
        }

        public void Populate(object o)
        {
            if (o.GetType() == typeof(NameValueCollection))
            {
                Populate((NameValueCollection)o);
                return;
            }
            var propertiesLocal = GetPropertiesWithSetter(this);
            var propertiesObject = GetProperties(o);
            foreach (var p in propertiesObject)
            {
                if (propertiesLocal.Where(s => s.Name == p.Name).Count() > 0)
                    SetPropertyValue(p.Name, p.Value);
            }
        }

        public string GetCustomFieldValue(string Reference, string Language)
        {
            return (string) this.GetType().GetMethod("GetFieldValue").Invoke(this, new object[] { Reference, Language });
        }

        public virtual IList<string> Populate(NameValueCollection data)
        {
            EntityExtensions.PopulateToProperties(this, data);
            return new List<string>();
        }

        public T Clone<T>(string lang) where T : BaseEntity,new()
        {
            var data = new T();
            data.Populate(this);
            data.SetPropertyValue("IdAppLanguage", lang);
            return data;
        }

        public bool HasProperty(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;
            Type t = this.GetType();
            return t.GetProperty(key) != null;
        }

        public void SetPropertyValue(string key, object o)
        {
            if (string.IsNullOrEmpty(key))
                return;
            Type t = this.GetType();
            var prop = t.GetProperty(key);
            if (prop != null)
                prop.SetValue(this, o);
        }

        public object GetPropertyValue(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;
            Type t = this.GetType();
            var prop = t.GetProperty(key);
            return prop != null ? prop.GetValue(this,null) : null;
        }

        public T GetPropertyValue<T>(string key) 
        {
            if (string.IsNullOrEmpty(key))
                return default(T);
            Type t = this.GetType();
            var prop = t.GetProperty(key);
            return prop != null ? (T)prop.GetValue(this) : default(T);
        }
    }

    public static class EntityExtensions
    {
        public static void PopulateToProperties(this AEntity obj, NameValueCollection data)
        {
            if (obj == null || data == null)
                return;
            var type = obj.GetType();
            var keys = data.AllKeys.Where(s => !s.StartsWith("_") && !s.StartsWith("$") && !s.StartsWith("["));
            foreach (string key in keys)
            {
                var p = type.GetProperty(key);
                if (p != null && !AEntity.PrivateFields.Contains(p.Name) && Utilities.Tools.IsPrimitive(p.PropertyType))
                    p.SetValue(obj, FieldsExtensions.Read(p.PropertyType, data[p.Name]));
            }
        }

        public static void PopulateToPropertiesByLanguage(this AEntity obj, NameValueCollection data, string lang)
        {
            if (obj == null || data == null || string.IsNullOrEmpty(lang))
                return;
            var type = obj.GetType();
            var reference = "[" + lang + "]";
            var keys = data.AllKeys.Where(s => s.StartsWith("[" + lang + "]"));
            foreach (string key in keys)
            {
                var p = type.GetProperty(key.Substring(reference.Length));
                if (p != null && !AEntity.PrivateFields.Contains(p.Name) && Utilities.Tools.IsPrimitive(p.PropertyType))
                    p.SetValue(obj, FieldsExtensions.Read(p.PropertyType, data[key]));
            }
        }

        public static class FieldsExtensions
        {
            public static object Read(Type original, string val)
            {
                if (original == typeof(DateTime) || original == typeof(DateTime?))
                    return ReadDate(original, val);
                if (original == typeof(int) || original == typeof(int?))
                    return ReadInteger(original, val);
                if (original == typeof(decimal) || original == typeof(decimal?))
                    return ReadDecimal(original, val);
                if (original == typeof(bool) || original == typeof(bool?))
                    return ReadBoolean(original, val);
                return val;
            }

            public static DateTime? ReadDate(Type original, string val)
            {
                if (string.IsNullOrEmpty(val) || string.IsNullOrEmpty(val.Trim()))
                    return (original == typeof(DateTime?)) ? (DateTime?)null : (DateTime?)DateTime.Now;
                return Configuration.Converter.ToDate(val);
            }

            public static int? ReadInteger(Type original, string val)
            {
                if (original == typeof(int?))
                    return string.IsNullOrEmpty(val) ? (int?)null : (int?)int.Parse(val);
                return string.IsNullOrEmpty(val) ? 0 : int.Parse(val);
            }

            public static decimal? ReadDecimal(Type original, string val)
            {
                if (string.IsNullOrEmpty(val))
                    return original == typeof(decimal?) ? (decimal?)null : (decimal?)0;
                if (val.Contains('.'))
                    return Configuration.Converter.ToDecimalCurrency(val);
                return Configuration.Converter.ToDecimalNumber(val);
            }

            public static bool? ReadBoolean(Type original, string val)
            {
                if (string.IsNullOrEmpty(val))
                    return original == typeof(bool?) ? (bool?)null : (bool?)false;
                return Configuration.Converter.ToBoolean(val);
            }
        }
    }

    public static class FieldsExtensions
    {
        public static object Read(Type original, string val)
        {
            if (original == typeof(DateTime) || original == typeof(DateTime?))
                return ReadDate(original, val);
            if (original == typeof(int) || original == typeof(int?))
                return ReadInteger(original, val);
            if (original == typeof(decimal) || original == typeof(decimal?))
                return ReadDecimal(original, val);
            if (original == typeof(bool) || original == typeof(bool?))
                return ReadBoolean(original, val);
            return val;
        }

        public static DateTime? ReadDate(Type original, string val)
        {
            if (string.IsNullOrEmpty(val) || string.IsNullOrEmpty(val.Trim()))
                return (original == typeof(DateTime?)) ? (DateTime?)null : (DateTime?)DateTime.Now;
            return Configuration.Converter.ToDate(val);
        }

        public static int? ReadInteger(Type original, string val)
        {
            if (original == typeof(int?))
                return string.IsNullOrEmpty(val) ? (int?)null : (int?)int.Parse(val);
            return string.IsNullOrEmpty(val) ? 0 : int.Parse(val);
        }

        public static decimal? ReadDecimal(Type original, string val)
        {
            if (string.IsNullOrEmpty(val))
                return original == typeof(decimal?) ? (decimal?)null : (decimal?)0;
            if (val.Contains('.'))
                return Configuration.Converter.ToDecimalCurrency(val);
            return Configuration.Converter.ToDecimalNumber(val);
        }

        public static bool? ReadBoolean(Type original, string val)
        {
            if (string.IsNullOrEmpty(val))
                return original == typeof(bool?) ? (bool?)null : (bool?)false;
            return Configuration.Converter.ToBoolean(val);
        }
    }
}
