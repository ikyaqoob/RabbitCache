using System;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using EasyNetQ;
using Moq;
using NUnit.Framework;
using RabbitCache.Extensions;
using RabbitCache.Helpers;

namespace RabbitCache.Test
{
    [TestFixture]
    public class ConfigurationTest : TestBase
    {
        [SetUp]
        public override void SetupFixture()
        {
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

            Assert.IsTrue(Configuration.WindsorContainer.Kernel.HasComponent(typeof(IBus)));
            Assert.IsTrue(Configuration.WindsorContainer.Kernel.HasComponent(typeof(IService)));
            Assert.IsTrue(Configuration.WindsorContainer.Kernel.HasComponent(typeof(ISerializer)));
            Assert.IsTrue(Configuration.WindsorContainer.Kernel.HasComponent(typeof(IConsumerErrorStrategy)));
        }
        [Test]
        public void InitializeCanResolveIServiceTest()
        {
            RabbitCache.Configuration.Initialize();

            var _resolve = Configuration.WindsorContainer.Resolve<IService>(new { _assembly = this.GetType().Assembly, _serviceName = "Test" });
            Assert.IsNotNull(_resolve);
        }
        [Test]
        public void InitializeCanResolveISerializerTest()
        {
            RabbitCache.Configuration.Initialize();

            var _resolve = Configuration.WindsorContainer.Resolve<ISerializer>();
            Assert.IsNotNull(_resolve);
        }
        [Test]
        public void InitializeCanResolveCacheIConsumerErrorStrategyyTest()
        {
            RabbitCache.Configuration.Initialize();

            var _resolve = Configuration.WindsorContainer.Resolve<IConsumerErrorStrategy>();
            Assert.IsNotNull(_resolve);
        }
        [Test]
        public void InitializeWhenIBusIsOverriddenTest()
        {
            var _bus = new Mock<IBus>().Object;
            var _windsorContainer = new WindsorContainer();
            _windsorContainer.
                Register(Component.For<IBus>().Instance(_bus).LifeStyle.Singleton.Override(Configuration.ServiceBusName));

            RabbitCache.Configuration.Initialize(_windsorContainer);

            Assert.AreEqual(_bus, _windsorContainer.Resolve<IBus>());
        }
        [Test]
        public void InitializeWhenIServiceIsOverriddenTest()
        {
            var _windsorContainer = new WindsorContainer();
            var _service = new Mock<IService>().Object;
            _windsorContainer
                .Register(Component.For<IService>().Instance(_service).LifeStyle.Transient.Override());

            RabbitCache.Configuration.Initialize(_windsorContainer);

            Assert.AreEqual(_service, _windsorContainer.Resolve<IService>());
        }
        [Test]
        public void InitializeWhenCacheEntryJsonSerializerIsOverriddenTest()
        {
            var _windsorContainer = new WindsorContainer();
            var _serializer = new Mock<ISerializer>().Object;
            _windsorContainer
                .Register(Component.For<ISerializer>().Instance(_serializer).LifeStyle.Transient.Override());

            RabbitCache.Configuration.Initialize(_windsorContainer);

            Assert.AreEqual(_serializer, _windsorContainer.Resolve<ISerializer>());
        }
        [Test]
        public void InitializeWhenCacheConsumerErrorStrategyIsOverriddenTest()
        {
            var _windsorContainer = new WindsorContainer();
            var _consumerErrorStrategy = new Mock<IConsumerErrorStrategy>().Object;
            _windsorContainer.Register(Component.For<IConsumerErrorStrategy>().Instance(_consumerErrorStrategy).LifeStyle.Transient.Override());

            RabbitCache.Configuration.Initialize(_windsorContainer);

            Assert.AreEqual(_consumerErrorStrategy, _windsorContainer.Resolve<IConsumerErrorStrategy>());
        }
        [Test]
        public void InitializeDoesNotThrowTest()
        {
            Assert.DoesNotThrow(() => Configuration.Initialize());
        }

        [Test]
        public void ShutdownWhenRabbitMqCacheBusTest()
        {
            var _bus = new Mock<IBus>().Object;
            var _windsorContainer = new WindsorContainer();
            _windsorContainer.
                Register(Component.For<IBus>().Instance(_bus).LifeStyle.Transient.Override(Configuration.ServiceBusName));

            RabbitCache.Configuration.Initialize(_windsorContainer);

            var _rabbitMqCacheBus = RabbitCache.Configuration.RabbitMqCacheBus;
            Assert.IsNotNull(_rabbitMqCacheBus);

            RabbitCache.Configuration.Shutdown();

            var _rabbitMqCacheBusField = typeof(Configuration).GetField("_rabbitMqCacheBus", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(_rabbitMqCacheBusField);

            var _rabbitMqCacheBusValue = _rabbitMqCacheBusField.GetValue(Configuration.Instance);
            Assert.IsNull(_rabbitMqCacheBusValue);
        }
        [Test]
        public void ShutdownWhenWindsorContainerTest()
        {
            var _bus = new Mock<IBus>().Object;
            var _windsorContainer = new WindsorContainer();
            _windsorContainer.
                Register(Component.For<IBus>().Instance(_bus).LifeStyle.Transient.Override(Configuration.ServiceBusName));

            RabbitCache.Configuration.Initialize(_windsorContainer);
            RabbitCache.Configuration.Shutdown();

            var _windsorContainerField = typeof(Configuration).GetField("_windsorContainer", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(_windsorContainerField);

            var _windsorContainerValue = _windsorContainerField.GetValue(Configuration.Instance);
            Assert.IsNull(_windsorContainerValue);
        }
        [Test]
        public void ShutdownDoesNotThrowTest()
        {
            Assert.DoesNotThrow(() => RabbitCache.Configuration.Shutdown());
        }
        
        [Test]
        public void GetPropertyRabbitMqCacheBusTest()
        {
            var _busMock = new Mock<IBus>();
            var _windsorContainer = new WindsorContainer();
            _windsorContainer.
                Register(Component.For<IBus>().Instance(_busMock.Object).LifeStyle.Transient.Override(Configuration.ServiceBusName));

            RabbitCache.Configuration.Initialize(_windsorContainer);

            Assert.AreEqual(_busMock.Object, Configuration.RabbitMqCacheBus);
        }
        [Test]
        public void GetPropertyRabbitMqCacheBusWhenNullTest()
        {
            var _busMock = new Mock<IBus>();
            var _windsorContainer = new WindsorContainer();
            _windsorContainer.
                Register(Component.For<IBus>().Instance(_busMock.Object).LifeStyle.Transient.Override(Configuration.ServiceBusName));

            RabbitCache.Configuration.Initialize(_windsorContainer);

            var _field = typeof(Configuration).GetField("_rabbitMqCacheBus", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(_field);
            _field.SetValue(Configuration.Instance, null);

            Assert.AreEqual(_busMock.Object, Configuration.RabbitMqCacheBus);
        }
        [Test]
        public void GetPropertyRabbitMqCacheBusWhenIsNotConnectedTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void GetPropertyWindsorContainerTest()
        {
            RabbitCache.Configuration.Initialize();
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