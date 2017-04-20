using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Application.Entities
{
    public interface IEntity
    {
        int Id { get; set; }
        DateTime DateCreated { get; set; }
        DateTime? DateUpdated { get; set; }
        bool IsNew();
    }

    public interface IEntityWithAttr
    {
        IList<string> Populate(NameValueCollection data);
        bool HasLanguageDefined(string lang);
        //IList<T> GetAttrs<T>() where T : IEntity;
    }

    public interface IEntityClonable
    {
        void Clone(string lang);
    }

    public interface IEntityCustomFieldRelation
    {
        int IdCustomField { get; set; }
        string Reference { get; set; }
        string IdAppLanguage { get; set; }
        string Value { get; set; }

        bool GetBoolValue();

        string GetTextValue();
    }
}
