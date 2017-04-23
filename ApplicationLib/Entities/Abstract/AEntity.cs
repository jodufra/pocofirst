using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Reflection;
using System.Collections;

namespace Application.Entities
{
    public abstract class AEntity : IEntity
    {
        public static List<string> PrivateFields = new List<string>() { "Id", "DateCreated", "DateUpdated" };

        public abstract int Id { get; set; }
        public abstract DateTime DateCreated { get; set; }
        public abstract DateTime? DateUpdated { get; set; }

        public bool IsNew()
        {
            return this.Id == 0;
        }
        
    }
    
    
}
