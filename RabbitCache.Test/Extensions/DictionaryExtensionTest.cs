using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using RabbitCache.Extensions;

namespace RabbitCache.Test.Extensions
{
    [TestFixture]
    public class GenericDictionaryExtensionTest
    {
        [Test]
        public void AddWhenTimoutTest()
        {
            var _objects = new Dictionary<object, object>();
            var _newObject = new object();

            Parallel.Invoke(
                () => _objects.Add(_newObject, new object(), 20, () => _objects.Remove(_newObject)),
                () =>
                {
                    Thread.Sleep(10);
                    Assert.IsTrue(_objects.ContainsKey(_newObject));
                    Thread.Sleep(100);
                }
                );

            Assert.IsFalse(_objects.ContainsKey(_newObject));
        }
        [Test]
        public void AddWhenTimoutNotElapsedTest()
        {
            var _objects = new Dictionary<object, object>();
            var _newObject = new KeyValuePair<object, object>();

            Parallel.Invoke(
                () => _objects.Add(_newObject, new object(), 20, () => _objects.Remove(_newObject)),
                () =>
                {
                    Thread.Sleep(10);
                    Assert.IsTrue(_objects.ContainsKey(_newObject));
                }
                );

            Assert.IsTrue(_objects.ContainsKey(_newObject));
        }
    }
}