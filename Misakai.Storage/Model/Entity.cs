using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Misakai.Storage
{
    /// <summary>
    /// Represents a contract information stored in EntityDB.
    /// </summary>
    public abstract class Entity
    {
        #region Properties
         /// <summary>
        /// Represents the object id of the meta object
        /// </summary>
        [Column("id"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key, Required]
        [JsonProperty("id")]
        public virtual int Oid
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets the date when the object was created
        /// </summary>
        [Column("created")]
        [JsonIgnore, Browsable(false)]
        public DateTime CreatedDate
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the date when the object was updated
        /// </summary>
        [Column("updated"), Index]
        [JsonIgnore, Browsable(false)]
        public DateTime UpdatedDate
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets a tag of the object.
        /// </summary>
        [Column("tag"), Index(IsUnique = true)]
        [JsonIgnore, Browsable(false)]
        public virtual string Tag
        {
            get;
            set;
        }
        
        #endregion

        #region Instance Members
        /// <summary>
        /// Saves the object to the underlying data store.
        /// </summary>
        /// <returns>Returns the updated date or an error.</returns>
        public virtual Expected<DateTime> TrySave()
        {
            try
            {
                using (var uow = EntityContext.Acquire())
                {
                    // Get the type
                    var type = ContextAttribute.GetContextType(this.GetType());

                    // Get the entry
                    var context = uow.DbContexts.Get(type);
                    var entry = context.Entry(this);
                    

                    // Is it a new object?
                    var isNew = entry.State == EntityState.Added || entry.State == EntityState.Detached;
                    if (entry.State == EntityState.Detached)
                        entry.State = EntityState.Added;
                }

                // We've successfully written this
                return this.UpdatedDate;
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Write(ex);

                // Something went wrong
                return ex;
            }
        }
        
        /// <summary>
        /// Deletes the object from the underlying data store.
        /// </summary>
        /// <returns>Returns the date of the deletion or an error.</returns>
        public virtual Expected<DateTime> TryDelete()
        {
            try
            {
                using (var uow = EntityContext.Acquire())
                {
                    // Get the type
                    var type = ContextAttribute.GetContextType(this.GetType());

                    // Get the entry
                    var context = uow.DbContexts.Get(type);
                    var entry   = context.Entry(this);

                    // Set as deleted
                    entry.State = EntityState.Deleted;
                    return DateTime.UtcNow;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Write(ex);

                // Something went wrong
                return ex;
            }
        }

        /// <summary>
        /// Converts the contract to the string format.
        /// </summary>
        /// <returns>The string representation of a contract</returns>
        public override string ToString()
        {
            try
            {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }
            catch
            {
                return base.ToString();
            }
        }
        #endregion
    }

}
