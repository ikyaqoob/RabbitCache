using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using RabbitCache.Extensions;

namespace RabbitCache.Test.Extensions
{
    [TestFixture]
    public class CollectionExtensionTest
    {
        [Test]
        public void AddWhenTimoutTest()
        {
            var _objects = new Collection<object>();
            var _newObject = new object();

            Parallel.Invoke(
                () => _objects.Add(_newObject, 50, () => _objects.Remove(_newObject)), 
                () =>
                    {
                        Thread.Sleep(10);
                        Assert.IsTrue(_objects.Contains(_newObject));
                        Thread.Sleep(150);
                    } 
                );
            
            Assert.IsFalse(_objects.Contains(_newObject));
        }
        [Test]
        public void AddWhenTimoutNotElapsedTest()
        {
            var _objects = new Collection<object>();
            var _newObject = new object();

            Parallel.Invoke(
                () => _objects.Add(_newObject, 50, () => _objects.Remove(_newObject)),
                () =>
                {
                    Thread.Sleep(10);
                    Assert.IsTrue(_objects.Contains(_newObject));
                }
                );

            Assert.IsTrue(_objects.Contains(_newObject));
        }
    }
}