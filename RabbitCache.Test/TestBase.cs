using System;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using EasyNetQ;
using EasyNetQ.Topology;
using Moq;
using NUnit.Framework;
using RabbitCache.Contracts;
using RabbitCache.Extensions;

namespace RabbitCache.Test
{
    public abstract class TestBase
    {
        [SetUp]
        public virtual void SetupFixture()
        {
            var _mock = new Mock<IBus> { CallBase = false };
            _mock.Setup(_x => _x.Advanced.Subscribe(It.IsAny<IQueue>(), It.IsAny<Func<IMessage<CacheEntry>, MessageReceivedInfo, Task>>()));

            var _windsorContainer = new WindsorContainer()
                .Register(Component.For<IBus>().Instance(_mock.Object).LifeStyle.Singleton.Override(Configuration.ServiceBusName));

            RabbitCache.Configuration.Initialize(_windsorContainer);
        }
        [TearDown]
        public virtual void TearDownextFixture()
        {
            RabbitCache.Configuration.Shutdown();
        }
    }
}