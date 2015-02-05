using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Core.Internal;
using EasyNetQ.Topology;
using log4net;
using RabbitCache.Caches.Interfaces;
using RabbitCache.Contracts.Interfaces;
using RabbitCache.Helpers;

namespace RabbitCache
{
    /// <summary>
    /// Factory class for creating Cache services.
    /// </summary>
    public class ServiceFactory : Singleton<ServiceFactory>
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ServiceFactory));

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ServiceFactory()
            : base()
        {
        }

        /// <summary>
        /// Creates and initializes a Service based on a <see cref="IService"/> implemenation.
        /// </summary>
        /// <param name="_service">The Service to Initialize</param>
        /// <returns>The IService initialized.</returns>
        public static IService CreateService(IService _service)
        {
            if (_service == null)
                throw new ArgumentNullException("_service");

            ServiceFactory.Instance.RegisterAll(_service);
            return _service;
        }
        /// <summary>
        /// Creates a new Cache Service.
        /// Several services can be created with different instantions of a Service and differnt name.
        /// The Service will Register all types as caches that implements <see cref="ICache"/>.
        /// Its recommended to store the Service returned in a Singleton, and use it throughout your application that way.
        /// </summary>
        /// <param name="_assembly">The Assembly containing types implementing <see cref="ICache"/>. if null the Assembly of <see cref="ICache"/> will be used.</param>
        /// <param name="_serviceName">Name of the Service. Must be unique among all Services created for the same server cluster.</param>
        /// <returns>The created IService.</returns>
        public static IService CreateService(Assembly _assembly, string _serviceName)
        {
            if (_assembly == null) 
                throw new ArgumentNullException("_assembly");

            if (_serviceName == null) 
                throw new ArgumentNullException("_serviceName");

            var _service = Configuration.WindsorContainer.Resolve<IService>(new { _assembly, _serviceName });
            ServiceFactory.Instance.RegisterAll(_service);

            return _service;
        }

        protected virtual void RegisterAll(IService _service)
        {
            if (_service == null)
                throw new ArgumentNullException("_service");

            var _cachedTypes = _service.Assembly.GetTypes().SelectMany(_x => _x.GetInterfaces()).Where(_x => _x.Name == (typeof(ICacheable<>).Name)).Distinct(); 
            _cachedTypes.ForEach(_x => 
                 {
                     var _type = _x.GenericTypeArguments.FirstOrDefault();
                     this.RegisterCache(_service, _type); 
                 }); 

            this.RegisterCacheQueueSubscription(_service);
        }
        protected virtual void RegisterCache(IService _service, Type _type)
        {
            if (_service == null) 
                throw new ArgumentNullException("_service");
            
            if (_type == null)
                throw new ArgumentNullException("_type");

            if (_service.Caches.ContainsKey(_type))
            {
                this._logger.InfoFormat("CacheType: {0} already registered. Ignoring.", _type);
                return;
            }

            var _cache = Activator.CreateInstance(_type) as ICache;
            _service.Caches.TryAdd(_type, _cache);

            this._logger.InfoFormat("CacheType: {0} registered", _type);
        }
        protected virtual void RegisterCacheQueueSubscription(IService _service)
        {
            if (_service == null) 
                throw new ArgumentNullException("_service");

            Configuration.RabbitMqCacheBus.Advanced.Subscribe<ICacheEntry>(this.GetQueue(_service), (_message, _info) => Task.Factory.StartNew(() =>
            {
                try
                {
                    _service.ReceiveCacheEntry(_message == null ? null : _message.Body);
                }
                catch (Exception _ex)
                {
                    this._logger.Info(_ex);
                }
            }));

            this._logger.Info("Cache RMQ Subscription Initialized");
        }
        protected virtual IQueue GetQueue(IService _service)
        {
            if (_service == null) 
                throw new ArgumentNullException("_service");

            var _exchange = Exchange.DeclareFanout(_service.Name);
            var _queue = Queue.Declare(false, true, true, _service.Name + "Queue:" + Guid.NewGuid().ToString("D"), null);
            _queue.BindTo(_exchange, _service.Name);

            return _queue;
        }
    }
}