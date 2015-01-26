using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using RabbitCache.Caches.Serialization;

namespace RabbitCache.Test.Caches.Serialization
{
    [TestFixture]
    public class CacheEntryContractResolverTest
    {
        [Test]
        public void InheritsFromDefaultContractResolverTest()
        {
            Assert.IsTrue(typeof(CacheEntryContractResolver).IsSubclassOf(typeof(DefaultContractResolver)));
        }
    }
}