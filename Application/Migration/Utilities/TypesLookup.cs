using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Application.Migration.Utilities
{
    public static class TypesLookup
    {
        public static IEnumerable<Type> GetTypesInNamespace(string nameSpace)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(t => t.GetTypes()).Where(t => t.IsClass && t.Namespace == nameSpace);
        }

        public static IEnumerable<Type> GetDataContractsInNamespace(string nameSpace, List<Type> excluded)
        {
            return GetTypesInNamespace(nameSpace).Where(c => !excluded.Contains(c) && Attribute.GetCustomAttribute(c, typeof(DataContractAttribute)) != null);
        }
    }

    public static class TypeExt
    {
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static string GetNullableName(this Type type)
        {
            return type.IsNullable() ? type.GetGenericArguments()[0].Name : type.Name;
        }
    }

    public static class PropertyInfoExt
    {

        public static bool IsNullable(this PropertyInfo info)
        {
            if (info.PropertyType != typeof(string))
                return info.PropertyType.IsNullable();
            var attribute = info.GetCustomAttribute(typeof(DataMemberAttribute), false) as DataMemberAttribute;
            return !attribute.IsRequired;
        }

    }
}
