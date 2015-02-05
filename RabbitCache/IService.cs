using System;
using System.Collections.Concurrent;
using System.Reflection;
using RabbitCache.Caches.Interfaces;
using RabbitCache.Contracts.Interfaces;

namespace RabbitCache
{
    /// <summary>
    /// 
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Name of the service.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Indicates if the Service is initialized.
        /// </summary>
        bool IsInitialized { get; }
        /// <summary>
        /// The assembly that was used to regsiter caches.
        /// </summary>
        Assembly Assembly { get; }
        /// <summary>
        /// The Caches defined by the service.
        /// </summary>
        ConcurrentDictionary<Type, ICache> Caches { get; }

        /// <summary>
        /// Get the cache matching Type of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Cache type definition.</typeparam>
        /// <returns>The Cache.</returns>
        T GetCache<T>();
        /// <summary>
        /// Gets the cache matching passed Type <paramref name="_type"/>.
        /// </summary>
        /// <param name="_type">Cache type definition.</param>
        /// <returns>The Cache.</returns>
        ICache GetCache(Type _type);
        /// <summary>
        /// Adds a new CacheEntry by passing it through the RabbitCache transport layer, pushing it to all subscribers. 
        /// </summary>
        /// <param name="_cacheEntry">The CacehEntry contract that will be send.</param>
        void AddCacheEntry(ICacheEntry _cacheEntry);
        /// <summary>
        /// Recieves a Cache Entry paassed through the RabbitCahce transport layer.
        /// This method does not need to be called explicitly when service is created using <see cref="ServiceFactory.CreateService(IService)"/>
        /// </summary>
        /// <param name="_cacheEntry">The CacehEntry contract received.</param>
        void ReceiveCacheEntry(ICacheEntry _cacheEntry);
    }
}