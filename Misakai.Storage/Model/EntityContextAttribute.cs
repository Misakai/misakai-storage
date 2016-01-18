using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Misakai.Storage
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ContextAttribute : Attribute
    {
        public ContextAttribute(Type contextType)
        {
            this.ContextType = contextType;
            if (!contextType.IsSubclassOf(typeof(EntityContext)))
                throw new ArgumentException("The contextType should be a subclass of EntityContext.");
        }

        public Type ContextType
        {
            get;
            set;
        }


        private readonly static ConcurrentDictionary<Type, Type> Map 
            = new ConcurrentDictionary<Type, Type>();


        public static Type GetContextType(Type entityType)
        {
            return Map.GetOrAdd(entityType, (k) =>
            {
                var attributes = entityType.GetCustomAttributes(typeof(ContextAttribute), true);
                if (attributes == null || attributes.Length == 0)
                    return null;

                return (attributes[0] as ContextAttribute).ContextType;
            });
        }
    }
}
