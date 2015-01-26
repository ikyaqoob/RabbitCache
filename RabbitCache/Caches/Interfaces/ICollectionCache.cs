using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace RabbitCache.Caches.Interfaces
{
    /// <summary>
    /// Represents an spatial cache and provides the base methods and properties for accessing the object cache.
    /// </summary>
    public interface ICollectionCache<TKey, TValue> : ICache, IEnumerable<KeyValuePair<TKey, IList<TValue>>>
        where TKey : class
        where TValue : class 
    {
        /// <summary>
        /// When overridden in a derived class, gets the total number of cache entries in the cache.
        /// </summary>
        /// <param name="_key"></param>
        /// <param name="_regionName">Optional. A named region in the cache for which the cache entry count should be computed, if regions are implemented. The default value for the optional parameter is null.</param>
        /// <returns>The number of cache entries in the cache. If regionName is not null, the count indicates the number of entries that are in the specified cache region.</returns>
        long GetCount(TKey _key, string _regionName = null);
        /// <summary>
        /// When overridden in a derived class, checks whether the cache entry already exists in the cache.
        /// </summary>
        /// <param name="_key">A unique identifier for the cache entry.</param>
        /// <param name="_value"></param>
        /// <param name="_regionName">Optional. A named region in the cache where the cache can be found, if regions are implemented. The default value for the optional parameter is null.</param>
        /// <returns>true if the cache contains a cache entry with the same key value as key; otherwise, false.</returns>
        bool Contains(TKey _key, TValue _value, string _regionName = null);
        /// <summary>
        /// When overridden in a derived class, gets the specified cache entry from the cache as an object.
        /// </summary>
        /// <param name="_key">A unique identifier for the cache entry to get.</param>
        /// <param name="_regionName">Optional. A named region in the cache to which the cache entry can be added, if regions are implemented. The default value for the optional parameter is null.</param>
        /// <returns>The cache entry that is identified by key.</returns>
        IEnumerable<TValue> Get(TKey _key, string _regionName = null);
        /// <summary>
        /// When overridden in a derived class, inserts the specified System.Runtime.Caching.CacheItem object into the cache, specifying information about how the entry will be evicted.
        /// </summary>
        /// <param name="_key">A unique identifier for the cache entry.</param>
        /// <param name="_value">The object to insert.</param>
        /// <param name="_regionName">Optional. A named region in the cache to which the cache entry can be added, if regions are implemented. The default value for the optional parameter is null.</param>
        /// <returns>If a cache entry with the same key exists, the specified cache entry; otherwise, null.</returns>
        TValue AddOrGetExisting(TKey _key, TValue _value, string _regionName = null);
        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache,
        /// specifying a key and a value for the cache entry, and information about how the entry will be evicted.
        /// </summary>
        /// <param name="_key">A unique identifier for the cache entry.</param>
        /// <param name="_value">The object to insert.</param>
        /// <param name="_policy">An object that contains eviction details for the cache entry. This object provides more options for eviction than a simple absolute expiration.</param>
        /// <param name="_regionName">Optional. A named region in the cache to which the cache entry can be added, if regions are implemented. The default value for the optional parameter is null.</param>
        /// <returns></returns>
        TValue AddOrGetExisting(TKey _key, TValue _value, CacheItemPolicy _policy, string _regionName = null);
        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache, by using a key, an object for the cache entry, an absolute expiration value,
        /// and an optional region to add the cache into.
        /// </summary>
        /// <param name="_key">A unique identifier for the cache entry.</param>
        /// <param name="_value">The object to insert.</param>
        /// <param name="_absoluteExpiration">The fixed date and time at which the cache entry will expire</param>
        /// <param name="_regionName">Optional. A named region in the cache to which the cache entry can be added, if regions are implemented. The default value for the optional parameter is null.</param>
        /// <returns>If a cache entry with the same key exists, the specified cache entry's value; otherwise, null.</returns>
        TValue AddOrGetExisting(TKey _key, TValue _value, DateTimeOffset? _absoluteExpiration, string _regionName = null);
        /// <summary>
        /// When overridden in a derived class, inserts or updates a cache entry into the cache, by using a key, an object for the cache entry, an absolute expiration value,
        /// and an optional region to add the cache into.
        /// </summary>
        /// <param name="_key"></param>
        /// <param name="_value"></param>
        /// <param name="_absoluteExpiration"></param>
        /// <param name="_regionName"></param>
        /// <returns></returns>
        TValue AddOrUpdateExisting(TKey _key, TValue _value, DateTimeOffset? _absoluteExpiration, string _regionName = null);
        /// <summary>
        /// When overridden in a derived class, removes the cache entry from the cache.
        /// </summary>
        /// <param name="_key">A unique identifier for the cache entry.</param>
        /// <param name="_value"></param>
        /// <param name="_regionName">Optional. A named region in the cache to which the cache entry can be added, if regions are implemented. The default value for the optional parameter is null.</param>
        void Remove(TKey _key, TValue _value, string _regionName = null);
        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache.
        /// </summary>
        /// <param name="_key">A unique identifier for the cache entry.</param>
        /// <param name="_value">The object to insert.</param>
        /// <param name="_regionName">Optional. A named region in the cache to which the cache entry can be added, if regions are implemented. The default value for the optional parameter is null.</param>
        TValue Update(TKey _key, TValue _value, string _regionName = null);
        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache.
        /// </summary>
        /// <param name="_key">A unique identifier for the cache entry.</param>
        /// <param name="_value">The object to insert.</param>
        /// <param name="_policy">An object that contains eviction details for the cache entry. This object provides more options for eviction than a simple absolute expiration.</param>
        /// <param name="_regionName">Optional. A named region in the cache to which the cache entry can be added, if regions are implemented. The default value for the optional parameter is null.</param>
        TValue Update(TKey _key, TValue _value, CacheItemPolicy _policy, string _regionName = null);
        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache, specifying time-based expiration details.
        /// </summary>
        /// <param name="_key">A unique identifier for the cache entry.</param>
        /// <param name="_value">The object to insert.</param>
        /// <param name="_absoluteExpiration">The fixed date and time at which the cache entry will expire.</param>
        /// <param name="_regionName">Optional. A named region in the cache to which the cache entry can be added, if regions are implemented. The default value for the optional parameter is null.</param>
        TValue Update(TKey _key, TValue _value, DateTimeOffset? _absoluteExpiration, string _regionName = null);
        /// <summary>
        /// When overridden in a derived class, gets a set of cache entries that correspond to the specified keys.
        /// </summary>
        /// <param name="_keys">A collection of unique identifiers for the cache entries to get.</param>
        /// <param name="_regionName">Optional. A named region in the cache to which the cache entry can be added, if regions are implemented. The default value for the optional parameter is null.</param>
        /// <returns>A dictionary of key/value pairs that represent cache entries.</returns>
        IDictionary<TKey, IEnumerable<TValue>> GetValues(IEnumerable<TKey> _keys, string _regionName = null);
        /// <summary>
        /// When overridden in a derived class, creates a System.Runtime.Caching.CacheEntryChangeMonitor
        /// object that can trigger events in response to changes to specified cache entries.
        /// </summary>
        /// <param name="_keys">The unique identifiers for cache entries to monitor.</param>
        /// <param name="_regionName">Optional. A named region in the cache to which the cache entry can be added, if regions are implemented. The default value for the optional parameter is null.</param>
        /// <returns></returns>
        CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(IEnumerable<TKey> _keys, string _regionName = null);
    }
}