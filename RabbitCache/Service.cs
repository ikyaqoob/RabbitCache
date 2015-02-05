using System;
using System.Collections.Concurrent;
using System.Reflection;
using EasyNetQ;
using EasyNetQ.Topology;
using log4net;
using RabbitCache.Caches.Interfaces;
using RabbitCache.Contracts.Interfaces;

namespace RabbitCache
{
    /// <summary>
    /// Service for Caching of user defined types implementing <see cref="ICache"/>.
    /// </summary>
    public class Service : IService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(Service));

        /// <summary>
        /// Name of the service. Must be Unique among all services.
        /// </summary>
        public virtual string Name { get; private set; }
        /// <summary>
        /// Indicates if the Service is initialized.
        /// </summary>
        public virtual bool IsInitialized { get; internal set; }
        /// <summary>
        /// The assembly that was used to regsiter caches.
        /// </summary>
        public virtual Assembly Assembly { get; private set; }
        /// <summary>
        /// The Caches defined by the service.
        /// </summary>
        public virtual ConcurrentDictionary<Type, ICache> Caches { get; private set; }

        /// <summary>
        /// Creates a Service. 
        /// NOTE: This will not add any RabbitCache transport layer setup. Use <see cref="ServiceFactory.CreateService(IService)"/>.
        /// </summary>
        /// <param name="_assembly">The Assembly to use for the Service</param>
        /// <param name="_serviceName">Unique name among all services.</param>
        public Service(Assembly _assembly, string _serviceName)
        {
            if (_assembly == null)
                throw new ArgumentNullException("_assembly");

            if (_serviceName == null)
                throw new ArgumentNullException("_serviceName");

            this.Name = _serviceName;
            this.Assembly = _assembly;
            this.IsInitialized = true;
            this.Caches = new ConcurrentDictionary<Type, ICache>();
        }

        /// <summary>
        /// Get the cache matching Type of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Cache type definition.</typeparam>
        /// <returns>The Cache.</returns>
        public virtual T GetCache<T>()
        {
            return (T)this.GetCache(typeof(T));
        }
        /// <summary>
        /// Gets the cache matching passed Type <paramref name="_type"/>.
        /// </summary>
        /// <param name="_type">Cache type definition.</param>
        /// <returns>The Cache.</returns>
        public virtual ICache GetCache(Type _type)
        {
            ICache _value;
            var _success = this.Caches.TryGetValue(_type, out _value);

            return _success ? _value : null;
        }
        /// <summary>
        /// Adds a new CacheEntry by passing it through the RabbitCache transport layer, pushing it to all subscribers. 
        /// </summary>
        /// <param name="_cacheEntry">The CacehEntry contract that will be send.</param>
        public virtual void AddCacheEntry(ICacheEntry _cacheEntry)
        {
            if (_cacheEntry == null)
                throw new ArgumentNullException("_cacheEntry");

            using (var _channel = Configuration.RabbitMqCacheBus.Advanced.OpenPublishChannel())
            {
                var _exchange = Exchange.DeclareFanout(this.Name);
                var _message = new Message<ICacheEntry>(_cacheEntry);

                _channel.Publish(_exchange, string.Empty, _message);
            }

            this._logger.DebugFormat("Published:{0}{1}", Environment.NewLine, _cacheEntry);
        }
        /// <summary>
        /// Recieves a Cache Entry paassed through the RabbitCahce transport layer.
        /// This method does not need to be called explicitly when service is created using <see cref="ServiceFactory.CreateService(IService)"/>
        /// </summary>
        /// <param name="_cacheEntry">The CacehEntry contract received.</param>
        public virtual void ReceiveCacheEntry(ICacheEntry _cacheEntry)
        {
            if (_cacheEntry == null)
                throw new ArgumentNullException("_cacheEntry");

            try
            {
                var _cache = this.GetCache(_cacheEntry.CacheType);
                if (_cache == null)
                {
                    this._logger.InfoFormat("Cache Type: {0}, undefined", _cacheEntry.CacheType);
                    return;
                }

                var _key = _cacheEntry.Key;
                var _value = _cacheEntry.Value;
                var _expireAt = _cacheEntry.ExpireAt;
                var _regionName = _cacheEntry.RegionName;

                _cache.AddOrUpdateExisting(_key, _value, _expireAt, _regionName);

                this._logger.DebugFormat("Received:{0}{1}", Environment.NewLine, _cacheEntry);
            }
            catch (Exception _ex)
            {
                this._logger.Info(_ex);
            }
        }
    }
}
