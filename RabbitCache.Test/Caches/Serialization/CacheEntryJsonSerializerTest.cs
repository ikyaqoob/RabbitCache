using EasyNetQ;
using NUnit.Framework;
using RabbitCache.Caches.Serialization;

namespace RabbitCache.Test.Caches.Serialization
{
    [TestFixture]
    public class CacheEntryJsonSerializerTest
    {
        [Test]
        public void ImplementsISerializerInterfaceTest()
        {
            Assert.IsTrue(typeof(ISerializer).IsAssignableFrom(typeof(CacheEntryJsonSerializer)));
        }

        [Test]
        public void BytesToMessageTest()
        {
            const string OBJECT = "Test";

            var _valueTypeJsonSerializer = new CacheEntryJsonSerializer();
            var _bytes = _valueTypeJsonSerializer.MessageToBytes(OBJECT);
            var _message = _valueTypeJsonSerializer.BytesToMessage<string>(_bytes);

            Assert.AreEqual(OBJECT, _message);
        }
        [Test]
        public void MessageToBytesTest()
        {
            const string OBJECT = "Test";
            var _valueTypeJsonSerializer = new CacheEntryJsonSerializer();
            var _message = _valueTypeJsonSerializer.MessageToBytes(OBJECT);

            Assert.AreEqual(OBJECT, _valueTypeJsonSerializer.BytesToMessage<string>(_message));
        }
   }
}