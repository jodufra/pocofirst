using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;
using Application.Infrastructure.Models;
using Application.Repositories;

namespace Application.Entities
{

    [DataContract]
    public class BaseEntity : AEntity
    {
        [DataMember]
        public override int Id { get; set; }
        [DataMember]
        public override DateTime DateCreated { get; set; }
        [DataMember]
        public override DateTime? DateUpdated { get; set; }
    }

    [DataContract]
    public class CustomFieldEntity : BaseEntity, IEntityCustomFieldRelation
    {
        [DataMember]
        public int IdCustomField { get; set; }

        [DataMember]
        public string Reference { get; set; }

        [DataMember]
        public string IdAppLanguage { get; set; }

        [DataMember]
        public string Value { get; set; }

        private CustomField _CustomField = null;
        public CustomField CustomField
        {
            get
            {
                if (_CustomField == null && IdCustomField > 0)
                    _CustomField = Application.Configuration.Resolver.GetRepository<CustomFieldRepository>().GetById(this.IdCustomField);
                return _CustomField;
            }
        }
        
        public IList<string> GetListValue()
        {
            return GetListValue(this.Value);
        }

        public static IList<string> GetListValue(string v)
        {
            return !string.IsNullOrEmpty(v) ? v.Split(',').ToList() : new List<string>();
        }

        public int GetIntegerValue()
        {
            return GetIntegerValue(this.Value);
        }

        public static int GetIntegerValue(string v)
        {
            return Configuration.Converter.ToInteger(v);
        }

        public decimal GetDecimalValue()
        {
            return GetDecimalValue(this.Value);
        }

        public static decimal GetDecimalValue(string v)
        {
            return Configuration.Converter.ToDecimalNumber(v);
        }

        public decimal GetCurrencyValue()
        {
            return GetCurrencyValue(this.Value);
        }

        public static decimal GetCurrencyValue(string v)
        {
            return Configuration.Converter.ToDecimalCurrency(v);
        }

        public bool GetBoolValue()
        {
            return GetBoolValue(this.Value);
        }

        public static bool GetBoolValue(string v)
        {
            return Configuration.Converter.ToBoolean(v);
        }

        public DateTime? GetDateValue()
        {
            return GetDateValue(this.Value);
        }

        public static Time GetTimeValue(string v)
        {
            return new Time(v);
        }

        public Time GetTimeValue()
        {
            return GetTimeValue(this.Value);
        }

        public static DateTime? GetDateValue(string v)
        {
            return Configuration.Converter.ToDate(v);
        }

        public string GetTextValue()
        {
            return GetTextValue(this.Value);
        }

        public static string GetTextValue(string v)
        {
            return v;
        }
    }
}
