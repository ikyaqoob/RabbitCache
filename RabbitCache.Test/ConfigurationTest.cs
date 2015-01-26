using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using EasyNetQ;
using Moq;
using NUnit.Framework;
using RabbitCache.Extensions;
using RabbitCache.Helpers;
using RabbitCache.Interfaces;

namespace RabbitCache.Test
{
    [TestFixture]
    public class ConfigurationTest
    {
        [TearDown]
        public virtual void TearDownextFixture()
        {
            RabbitCache.Configuration.Shutdown();
        }

        [Test]
        public void InheritsFromSingletonTest()
        {
            Assert.IsTrue(typeof(RabbitCache.Configuration).IsSubclassOf(typeof(Singleton<RabbitCache.Configuration>)));
        }
        [Test]
        public void TypeNameSerializerSerializeTest()
        {
            var _serialize = RabbitCache.Configuration.TypeNameSerializer.Serialize(typeof(object));
            Assert.AreEqual("System_Object", _serialize);
        }

        [Test]
        public void InitializeTest()
        {
            Configuration.Initialize();

            Assert.IsTrue(Configuration.WindsorContainer.Kernel.HasComponent(typeof(IService)));
            Assert.IsTrue(Configuration.WindsorContainer.Kernel.HasComponent(typeof(ISerializer)));
            Assert.IsTrue(Configuration.WindsorContainer.Kernel.HasComponent(typeof(IConsumerErrorStrategy)));
        }
        [Test]
        public void InitializeCanResolveIServiceTest()
        {
            Configuration.Initialize();

            var _resolve = Configuration.WindsorContainer.Resolve<IService>(new { _assembly = this.GetType().Assembly });
            Assert.IsNotNull(_resolve);
        }
        [Test]
        public void InitializeCanResolveISerializerTest()
        {
            Configuration.Initialize();

            var _resolve = Configuration.WindsorContainer.Resolve<ISerializer>();
            Assert.IsNotNull(_resolve);
        }
        [Test]
        public void InitializeCanResolveCacheIConsumerErrorStrategyyTest()
        {
            Configuration.Initialize();

            var _resolve = Configuration.WindsorContainer.Resolve<IConsumerErrorStrategy>();
            Assert.IsNotNull(_resolve);
        }
        [Test]
        public void InitializeWhenIBusIsOverriddenTest()
        {
            var _windsorContainer = new WindsorContainer();
            var _bus = new Mock<IBus>().Object;
            _windsorContainer.Register(Component.For<IBus>().Instance(_bus).LifeStyle.Transient.Override());

            Configuration.Initialize(_windsorContainer);

            Assert.AreEqual(_bus, _windsorContainer.Resolve<IBus>());
        }
        [Test]
        public void InitializeWhenIServiceIsOverriddenTest()
        {
            var _windsorContainer = new WindsorContainer();
            var _service = new Mock<IService>().Object;
            _windsorContainer.Register(Component.For<IService>().Instance(_service).LifeStyle.Transient.Override());

            Configuration.Initialize(_windsorContainer);

            Assert.AreEqual(_service, _windsorContainer.Resolve<IService>());
        }
        [Test]
        public void InitializeWhenCacheEntryJsonSerializerIsOverriddenTest()
        {
            var _windsorContainer = new WindsorContainer();
            var _serializer = new Mock<ISerializer>().Object;
            _windsorContainer.Register(Component.For<ISerializer>().Instance(_serializer).LifeStyle.Transient.Override());
            
            Configuration.Initialize(_windsorContainer);

            Assert.AreEqual(_serializer, _windsorContainer.Resolve<ISerializer>());
        }
        [Test]
        public void InitializeWhenCacheConsumerErrorStrategyIsOverriddenTest()
        {
            var _windsorContainer = new WindsorContainer();
            var _consumerErrorStrategy = new Mock<IConsumerErrorStrategy>().Object;
            _windsorContainer.Register(Component.For<IConsumerErrorStrategy>().Instance(_consumerErrorStrategy).LifeStyle.Transient.Override());

            Configuration.Initialize(_windsorContainer);

            Assert.AreEqual(_consumerErrorStrategy, _windsorContainer.Resolve<IConsumerErrorStrategy>());
        }
        [Test]
        public void InitializeDoesNotThrowTest()
        {
            Assert.DoesNotThrow(() => Configuration.Initialize());
        }

        [Test]
        public void ShutdownTest()
        {
            Assert.Inconclusive();
        }
        [Test]
        public void ShutdownWhenRabbitMqCacheBusTest()
        {
            Assert.Inconclusive();
        }
        [Test]
        public void ShutdownWhenWindsorContainerIsNullTest()
        {
            Assert.Inconclusive();
        }
        [Test]
        public void ShutdownDoesNotThrowTest()
        {
            Assert.DoesNotThrow(() => Configuration.Shutdown());
        }
        
        [Test]
        public void GetPropertyRabbitMqCacheBusTest()
        {
            Assert.Inconclusive();
        }
        [Test]
        public void GetPropertyRabbitMqCacheBusWhenNullTest()
        {
            Assert.Inconclusive();
        }
        [Test]
        public void GetPropertyRabbitMqCacheBusWhenIsNotConnectedTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void GetPropertyWindsorContainerTest()
        {
            Configuration.Initialize();
            Assert.DoesNotThrow(() =>
            {
                var _windsorContainer = Configuration.WindsorContainer;
                Assert.IsNotNull(_windsorContainer);
            });
        }
        [Test]
        public void GetPropertyWindsorContainerWhenNullTest()
        {
            IWindsorContainer _windsorContainer = null;
            var _exception = Assert.Throws<NullReferenceException>(() =>
            {
                _windsorContainer = Configuration.WindsorContainer;
            });

            Assert.AreEqual(_exception.Message, "_windsorContainer");
            Assert.IsNull(_windsorContainer);
        }
    }
}