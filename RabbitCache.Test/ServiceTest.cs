using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Moq;
using NUnit.Framework;
using RabbitCache.Contracts.Interfaces;
using RabbitCache.Extensions;
using RabbitCache.Interfaces;

namespace RabbitCache.Test
{
    [TestFixture]
    public class ServiceTest
    {
        [TearDown]
        public void TearDownextFixture()
        {
            RabbitCache.Configuration.Shutdown();
        }

        [Test]
        public void CreateServiceTest()
        {
            var _mock = new Mock<IService>();
            _mock.Setup(_x => _x.RegisterAll());

            var _windsorContainer = new WindsorContainer().Register(Component.For<IService>().Instance(_mock.Object).LifeStyle.Singleton.Override());
            RabbitCache.Configuration.Initialize(_windsorContainer);

            var _service = RabbitCache.Service.CreateService(typeof(object).Assembly);
            
            Assert.AreEqual(_mock.Object, _service);
            _mock.Verify(_x => _x.RegisterAll(), Times.Once());
        }
        
        [Test]
        public void GetCacheTest()
        {
            RabbitCache.Configuration.Initialize();

            var _service = RabbitCache.Service.CreateService(typeof(TestCache).Assembly);
            var _cache = _service.GetCache(typeof(TestCache));

            Assert.IsNotNull(_cache);
            Assert.AreEqual(_cache.GetType(), typeof(TestCache));
        }
        [Test]
        public void GetCacheTTest()
        {
            RabbitCache.Configuration.Initialize();

            var _service = RabbitCache.Service.CreateService(typeof(TestCache).Assembly);
            var _cache = _service.GetCache<TestCache>();

            Assert.IsNotNull(_cache);
            Assert.AreEqual(_cache.GetType(), typeof(TestCache));
        }
        [Test]
        public void RegisterAllTest()
        {
            Assert.Inconclusive();
        }
        [Test]
        public void AddCacheEntryTest()
        {
            var _serviceMock = new Mock<IService>();
            _serviceMock.Setup(_x => _x.AddCacheEntry(It.IsAny<ICacheEntry>()))
                .Callback<ICacheEntry>(_cacheEntry => _serviceMock.Object.ReceiveCacheEntry(_cacheEntry));

            var _windsorContainer = new WindsorContainer()
                .Register(Component.For<IService>().Instance(_serviceMock.Object).Override());

            Configuration.Initialize(_windsorContainer);

            var _cacheEntryMock = new Mock<ICacheEntry>();

            _serviceMock.Object.AddCacheEntry(_cacheEntryMock.Object);
            _serviceMock.Verify(_x => _x.AddCacheEntry(_cacheEntryMock.Object), Times.Once);
        }
        [Test]
        public void AddCacheEntryWhenNullTest()
        {
            Configuration.Initialize();
       
            var _service = Service.CreateService(this.GetType().Assembly);
            Assert.Throws<ArgumentNullException>(() => _service.AddCacheEntry(null));
        }
        [Test]
        public void ReceiveCacheEntryTest()
        {
            Assert.Inconclusive();
            //var _cacheEntryMock = new Mock<ICacheEntry>();
            //var _serviceMock = new Mock<IService>();
            //_serviceMock
            //    .Setup(_x => _x.GetCache<ICacheEntry>()).Returns(It.IsAny<ICacheEntry>());

            //_serviceMock
            //    .Setup(_x => _x.ReceiveCacheEntry(It.IsAny<ICacheEntry>()))
            //    .Callback<ICacheEntry>(_cacheEntry =>
            //    {
            //        var _cache = _serviceMock.Object.GetCache(_cacheEntry.CacheType);
                    
            //        if (_cache != null)
            //            _cache.AddOrUpdateExisting(_cacheEntry.Key, _cacheEntry.Value, _cacheEntry.ExpireAt, _cacheEntry.RegionName);
            //    });

            //var _windsorContainer = new WindsorContainer()
            //         .Register(Component.For<IService>().Instance(_serviceMock.Object).Override());
            
            //Configuration.Initialize(_windsorContainer);

            //_serviceMock.Object.ReceiveCacheEntry(_cacheEntryMock.Object);
            //_serviceMock.Verify(_x => _x.ReceiveCacheEntry(_cacheEntryMock.Object), Times.Once);
        }
        [Test]
        public void ReceiveCacheEntryWhenNullTest()
        {
            Configuration.Initialize();
   
            var _service = Service.CreateService(this.GetType().Assembly);
            Assert.Throws<ArgumentNullException>(() => _service.ReceiveCacheEntry(null));
        }
    }
}
