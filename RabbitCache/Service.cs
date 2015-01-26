using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Core.Internal;
using EasyNetQ;
using EasyNetQ.Topology;
using log4net;
using RabbitCache.Caches.Interfaces;
using RabbitCache.Contracts.Interfaces;
using RabbitCache.Interfaces;

namespace RabbitCache
{
    /// <summary>
    /// Service for Caching of user defined types implementing <see cref="ICache"/>.
    /// </summary>
    public class Service : IService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(Service));

        private bool _isInitialized;
        private readonly Assembly _assembly;
        private readonly ConcurrentDictionary<Type, ICache> _caches;

        /// <summary>
        /// Public constructor, required by by IoC.
        /// Use RabbitCache.Service.CreateService(Assembly) to probably create and initialize the Service.
        /// </summary>
        /// <param name="_assembly"></param>
        public Service(Assembly _assembly)
        {
            if (_assembly == null)
                throw new ArgumentNullException("_assembly");

            this._assembly = _assembly;
            this._caches = new ConcurrentDictionary<Type, ICache>();
        }

        /// <summary>
        /// Creates a new Cache Service.
        /// Several services can be created with differnt instantions of a Service.
        /// The Service will Register all types as caches that implements <see cref="ICache"/>.
        /// Its recommended to store the Service returned in a Singleton, and use it throughout your application that way.
        /// </summary>
        /// <param name="_assembly">The Assembly containing types implementing <see cref="ICache"/>. if null the Assembly of <see cref="ICache"/> will be used.</param>
        /// <returns></returns>
        public static IService CreateService(Assembly _assembly)
        {
            var _service = Configuration.WindsorContainer.Resolve<IService>(new { _assembly });
            _service.RegisterAll();

            return _service;
        }

        /// <summary>
        /// Gets the cache matching Type of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Cache type definition.</typeparam>
        /// <returns>The cache</returns>
        public virtual T GetCache<T>()
        {
            return (T)this.GetCache(typeof(T));
        }
        /// <summary>
        /// Gets the cache matching passed Type <paramref name="_type"/>.
        /// </summary>
        /// <param name="_type">Cache type definition.</param>
        /// <returns>The cache</returns>
        public virtual ICache GetCache(Type _type)
        {
            ICache _value;
            var _success = this._caches.TryGetValue(_type, out _value);

            return _success ? _value : null;
        }
        /// <summary>
        /// Registers all Types from the Assmebly used when the service was created, which implements ICache interface, as cachable.
        /// </summary>
        public virtual void RegisterAll()
        {
            if (this._isInitialized)
                return;

            this._assembly.GetTypes().Where(_x => _x.GetInterfaces().Any(_y => _y.Name == (typeof(ICache).Name)))
                .ForEach(this.RegisterCache);

            this.RegisterCacheQueueSubscription();
            this._isInitialized = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_cacheEntry"></param>
        public virtual void AddCacheEntry(ICacheEntry _cacheEntry)
        {
            if (_cacheEntry == null)
                throw new ArgumentNullException("_cacheEntry");

            using (var _channel = Configuration.RabbitMqCacheBus.Advanced.OpenPublishChannel())
            {
                var _serviceName = Service.GetServiceName();
                var _exchange = Exchange.DeclareFanout(_serviceName);
                var _message = new Message<ICacheEntry>(_cacheEntry);

                _channel.Publish(_exchange, string.Empty, _message);
            }

            this._logger.DebugFormat("Published:{0}{1}", Environment.NewLine, _cacheEntry);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_cacheEntry"></param>
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
  
        private void RegisterCache(Type _type)
        {
            if (_type == null)
                throw new ArgumentNullException("_type");

            if (this._caches.ContainsKey(_type))
            {
                this._logger.InfoFormat("CacheType: {0} already registered. Ignoring.", _type);
                return;
            }
            
            var _cache = Activator.CreateInstance(_type) as ICache;
            this._caches.TryAdd(_type, _cache);

            this._logger.InfoFormat("CacheType: {0} registered", _type);
        }
        private void RegisterCacheQueueSubscription()
        {  
            Configuration.RabbitMqCacheBus.Advanced.Subscribe<ICacheEntry>(Service.GetQueue(), (_message, _info) => Task.Factory.StartNew(() =>
                {
                    try
                    {
                        this.ReceiveCacheEntry(_message == null ? null : _message.Body);
                    }
                    catch (Exception _ex)
                    {
                        this._logger.Info(_ex);
                    }
                }));

            this._logger.Info("Cache RMQ Subscription Initialized");
        }
        private static IQueue GetQueue()
        {
            var _serviceName = Service.GetServiceName();
            var _exchange = Exchange.DeclareFanout(_serviceName);
            var _queue = Queue.Declare(false, true, true, _serviceName + "Queue:" + Guid.NewGuid().ToString("D"), null);
            _queue.BindTo(_exchange, _serviceName);

            return _queue;
        }
        private static string GetServiceName()
        {
            return ConfigurationManager.AppSettings["SERVICE_NAME"];
        }
    }
}
