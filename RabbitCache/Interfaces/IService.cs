using System;
using RabbitCache.Caches.Interfaces;
using RabbitCache.Contracts.Interfaces;

namespace RabbitCache.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Gets the cache matching Type of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Cache type definition.</typeparam>
        /// <returns>The cache</returns>
        T GetCache<T>();
        /// <summary>
        /// Gets the cache matching passed Type <paramref name="_type"/>.
        /// </summary>
        /// <param name="_type">Cache type definition.</param>
        /// <returns>The cache</returns>
        ICache GetCache(Type _type);
        /// <summary>
        /// Registers all Types from the Assmebly used when the service was created, which implements ICache interface, as cachable.
        /// </summary>
        void RegisterAll();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_cacheEntry"></param>
        void AddCacheEntry(ICacheEntry _cacheEntry);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_cacheEntry"></param>
        void ReceiveCacheEntry(ICacheEntry _cacheEntry);
    }
}