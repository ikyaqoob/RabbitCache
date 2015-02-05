# RabbitCache
Forward only distributed local in memory cache for spatial and regular collections.




Dont use this cache for you primary cache to offload database server transactions.
Use it for spefific needs where data changes often and you need to have a fast accesible layer of data, tailored to he need.

For example data about traffic changes in a maps application, where here the data often changes and updated.


Usage:
// -------------------------------------------------------------------------------------------------------------------------------
// Initialize your IoC.
var _myWindsorContainer = new WindsorContainer();
            
// Initialize RabbitCache Windsor Component Registration and Cache initialization. 
// * Also useful to override specific components used by RabbitCache for changed RabbitMQ Bus configuration or different serialization handling.
RabbitCache.Configuration.Initialize(_myWindsorContainer);

// The Assembly that contains implementation of ICacheable<> interface. All implementations of this interfaces will get a Cache setup durng Configuration Initialization.
var _myAssembly = Assembly.GetExecutingAssembly();

// Must be unique, in order for not mixing up caching in the Rabbit MQ exchange layer.
var _myServiceName = "MyRabbitCacheService";

// Create the Service and register the cache stores in _myAssembly.
var _service = RabbitCache.ServiceFactory.CreateService(_myAssembly, "MyRabbitCacheService");

// Add a new Cache Entry.
// * _service.ReceiveCacheEntry(ICacheEntry) is automatically called by all Subscribers. This is usually the same applicaton on a different server, so by using these libraries, its automatically subscriping and will recieve the CacheEntry.
_service.AddCacheEntry(new CacheEntry());

// Get CacheEntry and Query (Spatial)
var _spatialCache = _service.GetCache<SpatialCache<Coordinate, object, object>>();
var _spatialResult = _spatialCache.Query(new Coordinate(), 10);

// Get CacheEntry and Query (Spatial)
var _collectionCache = _service.GetCache<CollectionCache<object, object>>();
var _collectionCacheResult = _collectionCache.Get(new object());

// Shutdown RabbitCache. On application shutdown.
RabbitCache.Configuration.Shutdown();
// -------------------------------------------------------------------------------------------------------------------------------


Only by using the Service and Service Factory CacheEntries are passed through the Rabbit MQ message system.
Methods available on the ICache implementations are for different usage, and should be avoided unless you are injecting own Transport Layer and Different IService Implementation. 
IService will later be expanded to include an easier way of querying the underlying caches.



