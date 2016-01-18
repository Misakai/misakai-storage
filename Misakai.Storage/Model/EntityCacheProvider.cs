using System;
using System.Linq;

namespace Misakai.Storage
{
    /// <summary>
    /// This represents a provider of <see cref="Entity"/> types, backed by a caching layer.
    /// </summary>
    public class EntityCacheBaseProvider<TValue> : EntityObjectProvider<TValue>
        where TValue : Entity, new()
    {
        #region Constructors
        /// <summary>
        /// Constructs a new mongo provider, backed by a caching layer.
        /// </summary>
        /// <param name="session">The session to use for this provider</param>
        /// <param name="capacity">the normal item limit for cache (Count may exeed capacity due to minAge)</param>
        /// <param name="minAge">the minimium time after an access before an item becomes eligible for removal, during this time
        /// the item is protected and will not be removed from cache even if over capacity</param>
        /// <param name="maxAge">the max time that an object will sit in the cache without being accessed, before being removed</param>
        /// <param name="getByKeyFunc">The function that retrives the object by it's primary key.</param>
        public EntityCacheBaseProvider(IDbContextScopeFactory session, int capacity, TimeSpan minAge, TimeSpan maxAge, GetKeyFunc<int, TValue> getByKeyFunc)
            : base(session)
        {
            // Make sure we have the caching layer
            this.Cache = new LruCache<TValue>(capacity, minAge, maxAge, null);

            // Add the primary index
            this.ByKey = this.Cache.AddIndex<int>("oid", getByKeyFunc, this.FetchByKey);
            this.ByTag = this.Cache.AddIndex<string>("tag", (entity) => entity.Tag, this.FetchByTag);
        }



        /// <summary>
        /// Constructs a new mongo provider, backed by a caching layer.
        /// </summary>
        /// <param name="session">The session to use for this provider</param>
        /// <param name="capacity">the normal item limit for cache (Count may exeed capacity due to minAge)</param>
        /// <param name="getByKeyFunc">The function that retrives the object by it's primary key.</param>
        public EntityCacheBaseProvider(IDbContextScopeFactory session, int capacity, GetKeyFunc<int, TValue> getByKeyFunc)
            : this(session, capacity, TimeSpan.FromSeconds(20), TimeSpan.FromMinutes(30), getByKeyFunc) { }
        #endregion

        #region Cache Members
        /// <summary>
        /// Gets the cache layer.
        /// </summary>
        protected readonly LruCache<TValue> Cache = null;

        // Various indexing
        private LruCache<TValue>.IIndex<int> ByKey = null;
        private LruCache<TValue>.IIndex<string> ByTag = null;
        #endregion

        #region GetBy...() Members
        /// <summary>
        /// Gets an instance by a specific key from the underlying storage or cache. 
        /// </summary>
        /// <param name="key">The key to retrieve by.</param>
        /// <returns>An instance of the value for the specified key.</returns>
        public override Expected<TValue> GetByKey(int key)
        {
            try
            {
                // Load from cache, which will load from db using the provider
                return this.ByKey[key];
            }
            catch (Exception ex)
            {
                // If something failed during our process
                return ex;
            }
        }

        /// <summary>
        /// Gets an instance by a specific tag from the underlying storage or cache. 
        /// </summary>
        /// <param name="tag">The tag to retrieve by.</param>
        /// <returns>An instance of the value for the specified tag.</returns>
        public override Expected<TValue> GetByTag(string tag)
        {
            try
            {
                // Load from cache, which will load from db using the provider
                return this.ByTag[tag];
            }
            catch (Exception ex)
            {
                // If something failed during our process
                return ex;
            }
        }
        #endregion
    }

    /// <summary>
    /// This represents a provider of <see cref="Entity"/> types, backed by a caching layer.
    /// </summary>
    public class EntityCacheProvider<TValue> : EntityCacheBaseProvider<TValue>
        where TValue : Entity, new()
    {
        /// <summary>
        /// Constructs a new mongo provider, backed by a caching layer.
        /// </summary>
        /// <param name="session">The session to retrieve by.</param>
        /// <param name="capacity">the normal item limit for cache (Count may exeed capacity due to minAge)</param>
        /// <param name="minAge">the minimium time after an access before an item becomes eligible for removal, during this time
        /// the item is protected and will not be removed from cache even if over capacity</param>
        /// <param name="maxAge">the max time that an object will sit in the cache without being accessed, before being removed</param>
        public EntityCacheProvider(IDbContextScopeFactory session, int capacity, TimeSpan minAge, TimeSpan maxAge)
            : base(session, capacity, minAge, maxAge, (entity) => entity.Oid)
        {

        }

        /// <summary>
        /// Constructs a new mongo provider, backed by a caching layer.
        /// </summary>
        /// <param name="session">The session to retrieve by.</param>
        /// <param name="capacity">the normal item limit for cache (Count may exeed capacity due to minAge)</param>
        public EntityCacheProvider(IDbContextScopeFactory session, int capacity)
            : base(session, capacity, TimeSpan.FromSeconds(20), TimeSpan.FromMinutes(30), (entity) => entity.Oid)
        {

        }
    }
}
