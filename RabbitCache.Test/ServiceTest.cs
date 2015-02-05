using System;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using EasyNetQ;
using EasyNetQ.Topology;
using Moq;
using NUnit.Framework;
using RabbitCache.Contracts.Interfaces;
using RabbitCache.Extensions;

namespace RabbitCache.Test
{
    [TestFixture]
    public class ServiceTest : TestBase
    {
        [SetUp]
        public override void SetupFixture()
        {
        }

        [Test]
        public void GetCacheTest()
        {
            var _mockBus = new Mock<IBus> { CallBase = false };
            _mockBus.Setup(_x => _x.Advanced.Subscribe(It.IsAny<IQueue>(), It.IsAny<Func<IMessage<ICacheEntry>, MessageReceivedInfo, Task>>()));

            var _windsorContainer = new WindsorContainer()
                .Register(Component.For<IBus>().Instance(_mockBus.Object).LifeStyle.Singleton.Override(Configuration.ServiceBusName));

            RabbitCache.Configuration.Initialize(_windsorContainer);

            var _service = ServiceFactory.CreateService(this.GetType().Assembly, "Test");
            var _cache = _service.GetCache(typeof(TestCache));

            Assert.IsTrue(_service.Caches.ContainsKey(typeof(TestCache)));

            Assert.IsNotNull(_cache);
            Assert.AreEqual(_cache.GetType(), typeof(TestCache));
        }
        [Test]
        public void GetCacheTTest()
        {
            var _mockBus = new Mock<IBus> { CallBase = false };
            _mockBus.Setup(_x => _x.Advanced.Subscribe(It.IsAny<IQueue>(), It.IsAny<Func<IMessage<ICacheEntry>, MessageReceivedInfo, Task>>()));

            var _windsorContainer = new WindsorContainer()
                .Register(Component.For<IBus>().Instance(_mockBus.Object).LifeStyle.Singleton.Override(Configuration.ServiceBusName));

            RabbitCache.Configuration.Initialize(_windsorContainer);

            var _service = ServiceFactory.CreateService(this.GetType().Assembly, "Test");
            var _cache = _service.GetCache<TestCache>();

            Assert.IsNotNull(_cache);
            Assert.AreEqual(_cache.GetType(), typeof(TestCache));
        }
        [Test]
        public void AddCacheEntryTest()
        {
            var _mockBus = new Mock<IBus> { CallBase = false };
            _mockBus.Setup(_x => _x.Advanced.Subscribe(It.IsAny<IQueue>(), It.IsAny<Func<IMessage<ICacheEntry>, MessageReceivedInfo, Task>>()));

            var _serviceMock = new Mock<IService> { CallBase = false };
            _serviceMock.Setup(_x => _x.AddCacheEntry(It.IsAny<ICacheEntry>()))
                .Callback<ICacheEntry>(_cacheEntry => _serviceMock.Object.ReceiveCacheEntry(_cacheEntry));

            var _windsorContainer = new WindsorContainer()
                .Register(Component.For<IService>().Instance(_serviceMock.Object).Override())
                .Register(Component.For<IBus>().Instance(_mockBus.Object).LifeStyle.Singleton.Override(Configuration.ServiceBusName));

            RabbitCache.Configuration.Initialize(_windsorContainer);

            var _cacheEntryMock = new Mock<ICacheEntry>();
            _serviceMock.Object.AddCacheEntry(_cacheEntryMock.Object);
            _serviceMock.Verify(_x => _x.AddCacheEntry(_cacheEntryMock.Object), Times.Once);
            _serviceMock.Verify(_x => _x.ReceiveCacheEntry(_cacheEntryMock.Object), Times.Once);
        }
        [Test]
        public void AddCacheEntryWhenNullTest()
        {
            var _mockBus = new Mock<IBus> { CallBase = false };
            _mockBus.Setup(_x => _x.Advanced.Subscribe(It.IsAny<IQueue>(), It.IsAny<Func<IMessage<ICacheEntry>, MessageReceivedInfo, Task>>()));

            var _windsorContainer = new WindsorContainer()
                .Register(Component.For<IBus>().Instance(_mockBus.Object).LifeStyle.Singleton.Override(Configuration.ServiceBusName));

            RabbitCache.Configuration.Initialize(_windsorContainer);

            var _service = ServiceFactory.CreateService(this.GetType().Assembly, "Test");
            Assert.Throws<ArgumentNullException>(() => _service.AddCacheEntry(null));
        }
        [Test]
        public void ReceiveCacheEntryTest()
        {
            var _mockBus = new Mock<IBus> { CallBase = false };
            _mockBus.Setup(_x => _x.Advanced.Subscribe(It.IsAny<IQueue>(), It.IsAny<Func<IMessage<ICacheEntry>, MessageReceivedInfo, Task>>()));

            var _serviceMock = new Mock<IService> { CallBase = false };
            _serviceMock
                .Setup(_x => _x.ReceiveCacheEntry(It.IsAny<ICacheEntry>()))
                .Callback<ICacheEntry>(_cacheEntry =>
                {
                    var _cache = _serviceMock.Object.GetCache(_cacheEntry.CacheType);

                    if (_cache != null)
                        _cache.AddOrUpdateExisting(_cacheEntry.Key, _cacheEntry.Value, _cacheEntry.ExpireAt, _cacheEntry.RegionName);
                });
            
            var _windsorContainer = new WindsorContainer()
                .Register(Component.For<IService>().Instance(_serviceMock.Object))
                .Register(Component.For<IBus>().Instance(_mockBus.Object).LifeStyle.Singleton.Override(Configuration.ServiceBusName));

            RabbitCache.Configuration.Initialize(_windsorContainer);

            var _cacheEntryMock = new Mock<ICacheEntry>();

            _serviceMock.Object.ReceiveCacheEntry(_cacheEntryMock.Object);
            _serviceMock.Verify(_x => _x.ReceiveCacheEntry(_cacheEntryMock.Object), Times.Once);
        }
        [Test]
        public void ReceiveCacheEntryWhenNullTest()
        {
            var _mockBus = new Mock<IBus> { CallBase = false };
            _mockBus.Setup(_x => _x.Advanced.Subscribe(It.IsAny<IQueue>(), It.IsAny<Func<IMessage<ICacheEntry>, MessageReceivedInfo, Task>>()));

            var _windsorContainer = new WindsorContainer()
                .Register(Component.For<IBus>().Instance(_mockBus.Object).LifeStyle.Singleton.Override(Configuration.ServiceBusName));

            RabbitCache.Configuration.Initialize(_windsorContainer);
   
            var _service = ServiceFactory.CreateService(this.GetType().Assembly, "Test");
            Assert.Throws<ArgumentNullException>(() => _service.ReceiveCacheEntry(null));
        }
    }
}
