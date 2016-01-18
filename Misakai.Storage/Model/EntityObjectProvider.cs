using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace Misakai.Storage
{
    /// <summary>
    /// Represents a provider of <see cref="Entity"/> from the underlying datastore.
    /// </summary>
    public class EntityObjectProvider<TValue> : Provider
        where TValue : Entity, new()
    {
        #region Constructors
        /// <summary>
        /// The session used for underlying data access.
        /// </summary>
        protected readonly IDbContextScopeFactory Context;

        /// <summary>
        /// Constructs a new provider.
        /// </summary>
        public EntityObjectProvider(IDbContextScopeFactory context)
            : base()
        {
            this.Context = context;
        }
        #endregion

        #region GetBy...() Members
        /// <summary>
        /// Gets an instance by a specific tag from the underlying storage or cache. 
        /// </summary>
        /// <param name="tag">The tag to retrieve by.</param>
        /// <returns>An instance of the value for the specified tag.</returns>
        public virtual Expected<TValue> GetByTag(string tag)
        {
            try
            {
                // Load from cache, which will load from db using the provider
                return this.FetchByTag(tag);
            }
            catch (Exception ex)
            {
                // If something failed during our process
                return ex;
            }
        }

        /// <summary>
        /// Gets an instance by a specific key from the underlying storage or cache. 
        /// </summary>
        /// <param name="key">The key to retrieve by.</param>
        /// <returns>An instance of the value for the specified key.</returns>
        public virtual Expected<TValue> GetByKey(int key)
        {
            try
            {
                // Load from cache, which will load from db using the provider
                return this.FetchByKey(key);
            }
            catch (Exception ex)
            {
                // If something failed during our process
                return ex;
            }
        }
        #endregion

        #region GetManyBy...() Members
        /// <summary>
        /// Attempts to fetch a set of objects by their keys.
        /// </summary>
        /// <param name="keys">The keys to fetch.</param>
        /// <returns>The objects fetched, null or empty collection.</returns>
        public virtual Expected<IEnumerable<TValue>> GetManyByKey(IEnumerable<int> keys)
        {
            try
            {
                // Load from cache, which will load from db using the provider
                return this.FetchManyByKey(keys);
            }
            catch (Exception ex)
            {
                // If something failed during our process
                return ex;
            }
        }

        /// <summary>
        /// Attempts to fetch an object from the database using the key and a default session.
        /// </summary>
        /// <param name="tag">The object identifier.</param>
        /// <returns>The object fetched, or null.</returns>
        public virtual Expected<IEnumerable<TValue>> GetManyByTag(string tag)
        {
            try
            {
                // Load from cache, which will load from db using the provider
                return this.FetchManyByTag(tag);
            }
            catch (Exception ex)
            {
                // If something failed during our process
                return ex;
            }
        }
        #endregion

        #region GetOrCreate...() Members
        /// <summary>
        /// Gets a data object by the key and locks it. If no data-object was found
        /// a new one is created with the specified key.
        /// </summary>
        /// <param name="key">TValuehe key of the data object to get.</param>
        /// <returns>TValuehe data object retrieved and locked.</returns>
        public Expected<TValue> GetOrCreate(int key)
        {
            // Try to get first
            var done = this.GetByKey(key);
            if (!done.IsEmpty)
                return done;

            // Create a new instance 
            var obj = new TValue();
            return obj;
        }

        /// <summary>
        /// Gets a data object by the key and locks it. If no data-object was found
        /// a new one is created with the specified key.
        /// </summary>
        /// <param name="key">TValuehe key of the data object to get.</param>
        /// <param name="construct">Constructor func.</param>
        /// <returns>TValuehe data object retrieved and locked.</returns>
        public Expected<TValue> GetOrCreate(int key, Func<int, TValue> construct)
        {
            var done = this.GetByKey(key);
            if (!done.IsEmpty)
                return done;

            // Create a new instance 
            var obj = construct(key);
            return obj;
        }

        /// <summary>
        /// Gets a data object by the tag and locks it. If no data-object was found
        /// a new one is created with the specified key.
        /// </summary>
        /// <param name="tag">TValuehe tag of the data object to get.</param>
        /// <returns>TValuehe data object retrieved and locked.</returns>
        public Expected<TValue> GetOrCreateByTag(string tag)
        {
            var done = this.GetByTag(tag);
            if (!done.IsEmpty)
                return done;

            // Create a new instance 
            var obj = new TValue();
            obj.Tag = tag;
            return obj;
        }

        /// <summary>
        /// Gets a data object by the tag and locks it. If no data-object was found
        /// a new one is created with the specified key.
        /// </summary>
        /// <param name="tag">TValuehe tag of the data object to get.</param>
        /// <param name="construct">Consturtor function.</param>
        /// <returns>TValuehe data object retrieved and locked.</returns>
        public Expected<TValue> GetOrCreateByTag(string tag, Func<string, TValue> construct)
        {
            var done = this.GetByTag(tag);
            if (!done.IsEmpty)
                return done;

            // Create a new instance 
            var obj = construct(tag);
            obj.Tag = tag;
            return obj;
        }

        /// <summary>
        /// Creates an object if it is not present in the database.
        /// </summary>
        /// <param name="key">TValuehe key of the object to check.</param>
        /// <param name="construct">TValuehe construct to execute in order to construct the object.</param>
        /// <returns>Whether the object was created or not.</returns>
        public Expected<TValue> CreateIfNull(int key, Func<TValue> construct)
        {
            var done = this.GetByKey(key);
            if (!done.IsEmpty)
                return done;

            // Construct it and commit
            TValue item = construct();
            item.TrySave();
            return item;
        }
        #endregion

        #region Fetch Members
        /// <summary>
        /// Gets an instance of a <see cref="Entity"/> from the underlying data storage.
        /// </summary>
        /// <param name="key">The key the value by.</param>
        /// <returns>An instance of the value for the provided key.</returns>
        protected TValue FetchByKey(int key)
        {
            try
            {
                // Get from the session object
                using (var uow = EntityContext.Acquire())
                {
                    // Get the type
                    var type = ContextAttribute.GetContextType(typeof(TValue));

                    // Get the context and the collection
                    var context = uow.DbContexts.Get(type);
                    var collection = context.Set<TValue>();

                    // Find
                    return collection.Find(key);
                }
            }
            catch (Exception ex)
            {
                // Log the exception and return null
                Log.Write(ex);
                return default(TValue);
            }
        }

        /// <summary>
        /// Gets an instance of a <see cref="Entity"/> from the underlying data storage.
        /// </summary>
        /// <param name="tag">The tag the value by.</param>
        /// <returns>An instance of the value for the provided key.</returns>
        protected TValue FetchByTag(string tag)
        {
            try
            {
                return this.FetchByQuery()
                    .Where(o => o.Tag == tag)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Log the exception and return null
                Log.Write(ex);
                return default(TValue);
            }
        }

        /// <summary>
        /// Gets all instances of a <see cref="Entity"/> from the underlying data storage.
        /// </summary>
        /// <param name="tag">The tag the value by.</param>
        /// <returns>An instance of the value for the provided key.</returns>
        protected TValue[] FetchManyByTag(string tag)
        {
            try
            {
                return this.FetchByQuery()
                    .Where(t => t.Tag == tag)
                    .ToArray();
            }
            catch (Exception ex)
            {
                // Log the exception and return null
                Log.Write(ex);
                return new TValue[0];
            }
        }

        /// <summary>
        /// Attempts to fetch a set of objects by their keys.
        /// </summary>
        /// <param name="keys">The keys to fetch.</param>
        /// <returns>The objects fetc</returns>
        protected TValue[] FetchManyByKey(IEnumerable<int> keys)
        {
            try
            {
                return this.FetchByQuery()
                    .Where(e => keys.Contains(e.Oid))
                    .ToArray();
            }
            catch (Exception ex)
            {
                // Log the exception and return null
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        /// Creates a new query on the underlying data storage.
        /// </summary>
        /// <returns>The created query.</returns>
        protected IQueryable<TValue> FetchByQuery()
        {
            try
            {
                // Get from the session object
                using (var uow = EntityContext.Acquire())
                {
                    // Get the type
                    var type = ContextAttribute.GetContextType(typeof(TValue));

                    // Get the context and the collection
                    var context = uow.DbContexts.Get(type);
                    var collection = context.Set<TValue>();

                    // Return the collection
                    return collection;
                }
            }
            catch (Exception ex)
            {
                // Log the exception and return null
                Log.Write(ex);
                return null;
            }
        }

        #endregion

    }
}
