using System.Threading;
using System.Threading.Tasks;
using GeoAPI.Geometries;
using NetTopologySuite.Index.Quadtree;
using NUnit.Framework;
using RabbitCache.Extensions;

namespace RabbitCache.Test.Extensions
{
    [TestFixture]
    public class QuadtreeExtensionTest
    {
        [Test]
        public void ContainsTest()
        {
            var _objects = new Quadtree<object>();
            var _newObject = new object();

            var _envelope = new Envelope(new Coordinate(1, 1));
            _objects.Add(_envelope, _newObject);

            Assert.IsTrue(_objects.Contains(_envelope));
        }
        [Test]
        public void ContainsWheNotExistsTest()
        {
            var _objects = new Quadtree<object>();
            var _envelope = new Envelope(new Coordinate(1, 1));

            Assert.IsFalse(_objects.Contains(_envelope));
        }

        [Test]
        public void AddWhenTimoutTest()
        {
            var _objects = new Quadtree<object>();
            var _newObject = new object();

            var _envelope = new Envelope(new Coordinate(1, 1));

            Parallel.Invoke(
                () => _objects.Add(_envelope, _newObject, 50, () => _objects.Remove(_envelope, _newObject)), 
                () =>
                    {
                        Thread.Sleep(10);
                        Assert.IsTrue(_objects.Contains(_envelope));
                        Thread.Sleep(100);
                    }
                );

            Assert.IsFalse(_objects.Contains(_envelope));
        }
        [Test]
        public void AddWhenTimoutNotElapsedTest()
        {
            var _objects = new Quadtree<object>();
            var _newObject = new object();

            var _envelope = new Envelope(new Coordinate(1, 1));

            Parallel.Invoke(
                () => _objects.Add(_envelope, _newObject, 100, () => _objects.Remove(_envelope, _newObject)),
                () =>
                    {
                        Thread.Sleep(10);
                        Assert.IsTrue(_objects.Contains(_envelope));
                    }
                );

            Assert.IsTrue(_objects.Contains(_envelope));
        }
        [Test]
        public void AddWhenTimoutAndMultipleObjectsTest()
        {
            var _objects = new Quadtree<object>();
            var _newObject = new object();
            var _newObject2 = new object();

            var _envelope = new Envelope(new Coordinate(1, 1));
            var _envelope2 = new Envelope(new Coordinate(2, 2));

            Parallel.Invoke(
                () =>
                    {
                        _objects.Add(_envelope, _newObject, 100, () => _objects.Remove(_envelope, _newObject));
                        _objects.Add(_envelope2, _newObject2, 500, () => _objects.Remove(_envelope2, _newObject2));
                    },
                () =>
                    {
                        Thread.Sleep(10);

                        Assert.IsTrue(_objects.Contains(_envelope));
                        Assert.IsTrue(_objects.Contains(_envelope2));

                        Thread.Sleep(250);
                    }
                );

            Assert.IsFalse(_objects.Contains(_envelope));
            Assert.IsTrue(_objects.Contains(_envelope2));

        }
    }
}