using NUnit.Framework;

namespace RabbitCache.Test
{
    [TestFixture]
    public class ServiceFactoryTest : TestBase
    {
        [Test]
        public void CreateServiceTest()
        {
            const string NAME = "Test";
            var _assembly = typeof(TestCache).Assembly;
            var _service = RabbitCache.ServiceFactory.CreateService(_assembly, "Test");

            Assert.IsTrue(_service.IsInitialized);
            Assert.IsTrue(_service.Caches.ContainsKey(typeof(TestCache)));
            Assert.AreEqual(NAME, _service.Name);
            Assert.AreEqual(_assembly, _service.Assembly);
        }
    }
}