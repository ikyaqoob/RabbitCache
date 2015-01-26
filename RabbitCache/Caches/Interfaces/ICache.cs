using System;
using System.Runtime.Caching;

namespace RabbitCache.Caches.Interfaces
{
    public partial interface ICache
    {
        /// <summary>
        /// Gets the name of a specific Cache instance. The name of a specific cache instance.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// A bitwise combination of flags that indicate the default capabilities of a cache implementation.
        /// When overridden in a derived class, gets a description of the features that a cache implementation provides.
        /// </summary>
        DefaultCacheCapabilities DefaultCacheCapabilities { get; }

        /// <summary>
        /// Add or gets an existing object.
        /// </summary>
        /// <param name="_key"></param>
        /// <param name="_value"></param>
        /// <param name="_absoluteExpiration"></param>
        /// <param name="_regionName"></param>
        /// <returns>The added object, or existing.</returns>
        object AddOrGetExisting(object _key, object _value, DateTimeOffset? _absoluteExpiration = null, string _regionName = null);
        /// <summary>
        /// Adds or Updates an existing object.
        /// </summary>
        /// <param name="_objectKeyValue"></param>
        /// <param name="_value"></param>
        /// <param name="_absoluteExpiration"></param>
        /// <param name="_regionName"></param>
        /// <returns>The added or updated object.</returns>
        object AddOrUpdateExisting(object _objectKeyValue, object _value, DateTimeOffset? _absoluteExpiration = null, string _regionName = null);
    }
}