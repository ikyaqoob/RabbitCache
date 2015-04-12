using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;
using NUnit.Framework;
using RabbitCache.Caches;
using RabbitCache.Caches.Entities;
using RabbitCache.Caches.Entities.Interfaces;
using Coordinate = RabbitCache.Caches.Entities.Coordinate;

namespace RabbitCache.Test.Caches
{
    [TestFixture]
    public class SpatialCacheTest
    {
        [Test]
        public void GetCountTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 1.11, Y = 1.11 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _count = _cache.GetCount();
            Assert.AreEqual(1, _count);
        }
        [Test]
        public void GetCountWhenNoElementsTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();

            var _count = _cache.GetCount();
            Assert.AreEqual(0, _count);
        }
        [Test]
        public void GetCountWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 1.22, Y = 1.22 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _count = _cache.GetCount(REGION);
            Assert.AreEqual(1, _count);
        }
        [Test]
        public void GetCountWhenRegionAndNoElementsTest()
        {
            const string REGION = "Test";

            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 1.33, Y = 1.33 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _count = _cache.GetCount();
            Assert.AreEqual(0, _count);
        }
        [Test]
        public void GetCountWhenRegionNameDoesNotExistsTest()
        {
            const string REGION = "Test";
            const string REGION2 = "Test2";

            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 1.44, Y = 1.44 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            Assert.Throws<KeyNotFoundException>(() => _cache.GetCount(REGION2));
        }

        [Test]
        public void ContainsTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 1.55, Y = 1.55 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _contains = _cache.Contains(_key);
            Assert.IsTrue(_contains);
        }
        [Test]
        public void ContainsWhenFalseTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 1.66, Y = 1.66 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _key2 = new Coordinate { X = 1.664, Y = 1.664 };
            var _contains = _cache.Contains(_key2);
            Assert.IsFalse(_contains);
        }
        [Test]
        public void ContainsWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 1.77, Y = 1.77 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _contains = _cache.Contains(_key, REGION);
            Assert.IsTrue(_contains);
        }
        [Test]
        public void ContainsWhenRegionNameAndFalseTest()
        {
            const string REGION = "Test";

            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 1.88, Y = 1.88 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _contains = _cache.Contains(_key);
            Assert.IsFalse(_contains);
        }
        [Test]
        public void ContainsWhenRegionNameDoesNotExistsTest()
        {
            const string REGION = "Test";
            const string REGION2 = "Test2";

            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 1.99, Y = 1.99 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _contains = _cache.Contains(_key, REGION2);
            Assert.IsFalse(_contains);
        }

        [Test]
        public void GetTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 2.11, Y = 2.11 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _actual = _cache.Get(_key);
            Assert.AreEqual(_spatialItem, _actual);
        }
        [Test]
        public void GetWhenNotExistsTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 2.22, Y = 2.22 };

            var _actual = _cache.Get(_key);
            Assert.IsNull(_actual);
        }
        [Test]
        public void GetWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 2.33, Y = 2.33 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _actual = _cache.Get(_key, REGION);
            Assert.AreEqual(_spatialItem, _actual);
        }
        [Test]
        public void GetWhenRegionNameAndNotExistsTest()
        {
            const string REGION = "Test2.44";

            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 2.44, Y = 2.44 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _key2 = new Coordinate { X = 12.445, Y = 12.445 };
            var _actual = _cache.Get(_key2, REGION);
            Assert.IsNull(_actual);
        }
        [Test]
        public void GetWhenRegionNameDoesNotExistsTest()
        {
            const string REGION = "Test";
            const string REGION2 = "Test2";

            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 2.55, Y = 2.55 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _cacheItem = _cache.Get(_key, REGION2);
            Assert.IsNull(_cacheItem);
        }
        [Test]
        public void GetStressTest()
        {
            const int LOOPS1 = 3;
            const int LOOPS2 = 500;

            var _dictionary = new Dictionary<Coordinate, object>();
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();

            for (var _j = 1; _j <= LOOPS2; _j++)
            {
                var _key = new Coordinate { X = _j, Y = _j};
                var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key, SpatialValue = new object(), ObjectKeyValue = new TestCacheObject() };
                _cache.AddOrGetExisting(_key, _spatialItem);
                _dictionary.Add(_key, _spatialItem);
            }

            for (var _i = 0; _i < LOOPS1; _i++)
            {
                var _dateTimeOffSet = DateTimeOffset.UtcNow;

                for (var _j = 1; _j <= LOOPS2; _j++)
                {
                    var _key = new Coordinate { X = _j, Y = _j };
                    _cache.Get(_key);
                }

                var _elapsedTime = (DateTimeOffset.UtcNow - _dateTimeOffSet).TotalMilliseconds;
                Assert.LessOrEqual(_elapsedTime, 30);
            }
        }
 
        [Test]
        public void AddOrGetExistingTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 2.66, Y = 2.66 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);
        }
        [Test]
        public void AddOrGetExistingWhenNotExistsTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 2.66, Y = 2.66 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);
        }
        [Test]
        public void AddOrGetExistingWithObjectTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 2.66, Y = 2.66 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting((object)_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);
        }
        [Test]
        public void AddOrGetExistingWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 2.77, Y = 2.77 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);
        }
        [Test]
        public void AddOrGetExistingWhenCacheItemPolicyTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 2.88, Y = 2.88 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.UtcNow.AddMilliseconds(20) });
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            Thread.Sleep(80);

            var _actual = _cache.Get(_key);
            Assert.IsNull(_actual);
        }
        [Test]
        public void AddOrGetExistingWhenAbsoluteExpirationTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 2.99, Y = 2.99 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, DateTimeOffset.UtcNow.AddMilliseconds(20));
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            Thread.Sleep(80);

            var _actual = _cache.Get(_key);
            Assert.IsNull(_actual);
        }
        [Test]
        public void AddOrGetExistingStressTest()
        {
            const int LOOPS1 = 3;
            const int LOOPS2 = 500;

            for (var _i = 0; _i < LOOPS1; _i++)
            {
                var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
                var _dateTimeOffSet = DateTimeOffset.UtcNow;

                for (var _j = 1; _j <= LOOPS2; _j++)
                {
                    var _key = new Coordinate { X = _j, Y = _j };
                    var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key, SpatialValue = new object(), ObjectKeyValue = new TestCacheObject() };
                    _cache.AddOrGetExisting(_key, _spatialItem);
                }

                var _elapsedTime = (DateTimeOffset.UtcNow - _dateTimeOffSet).TotalMilliseconds;
                Assert.LessOrEqual(_elapsedTime, 50);
            }
        }
        [Test]
        public void AddOrGetExistingStressAsyncTest()
        {
            const int LOOPS1 = 3;
            const int LOOPS2 = 500;

            for (var _i = 0; _i < LOOPS1; _i++)
            {
                var _dictionary = new Dictionary<Coordinate, SpatialCacheItem<Coordinate, object, TestCacheObject>>();
                var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();

                for (var _j = 1; _j <= LOOPS2; _j++)
                {
                    var _key = new Coordinate { X = _j, Y = _j };
                    var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key, SpatialValue = new object(), ObjectKeyValue = new TestCacheObject() };
                    _dictionary.Add(_key, _spatialItem);
                }

                var _dateTimeOffSet = DateTimeOffset.UtcNow;
                Parallel.ForEach(_dictionary, _x => _cache.AddOrGetExisting(_x.Key, _x.Value));

                var _elapsedTime = (DateTimeOffset.UtcNow - _dateTimeOffSet).TotalMilliseconds;
                Assert.LessOrEqual(_elapsedTime, 150);
            }
        }
        [Test]
        public void AddOrGetExistingStressAsyncWhenFastTimeoutTest()
        {
            const int LOOPS1 = 3;
            const int LOOPS2 = 500;

            for (var _i = 0; _i < LOOPS1; _i++)
            {
                var _dictionary = new Dictionary<Coordinate, SpatialCacheItem<Coordinate, object, TestCacheObject>>();
                var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();

                for (var _j = 1; _j <= LOOPS2; _j++)
                {
                    var _key = new Coordinate { X = _j, Y = _j };
                    var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key, SpatialValue = new object(), ObjectKeyValue = new TestCacheObject() };
                    _dictionary.Add(_key, _spatialItem);
                }

                var _dateTimeOffSet = DateTimeOffset.UtcNow;
                Parallel.ForEach(_dictionary, _x => _cache.AddOrGetExisting(_x.Key, _x.Value, DateTimeOffset.UtcNow.AddMilliseconds(50)));

                var _elapsedTime = (DateTimeOffset.UtcNow - _dateTimeOffSet).TotalMilliseconds;
                Assert.LessOrEqual(_elapsedTime, 50);
            }
        }

        [Test]
        public void AddOrUpdateExistingTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 3.11, Y = 3.11 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key, ObjectKeyValue = new TestCacheObject() };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _key2 = new Coordinate { X = 3.115, Y = 3.115 };
            var _changedSpatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key2, ObjectKeyValue = new TestCacheObject() };
            var _addOrUpdateExisting = _cache.AddOrUpdateExisting(_spatialItem.ObjectKeyValue, _changedSpatialItem);

            Assert.AreNotEqual(_addOrGetExisting, _addOrUpdateExisting);
            Assert.AreEqual(_changedSpatialItem, _addOrUpdateExisting);
        }
        [Test]
        public void AddOrUpdateExistingWhenNotExistsTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 2.66, Y = 2.66 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key, ObjectKeyValue = new TestCacheObject() };

            var _addOrUpdateExisting = _cache.AddOrUpdateExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrUpdateExisting);
        }
        [Test]
        public void AddOrUpdateExistingWithObjectTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 2.66, Y = 2.66 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key, ObjectKeyValue = new TestCacheObject() };

            var _addOrUpdateExisting = _cache.AddOrUpdateExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrUpdateExisting);
        }
        [Test]
        public void AddOrUpdateExistingWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 3.22, Y = 3.22 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key, ObjectKeyValue = new TestCacheObject() };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _key2 = new Coordinate { X = 3.225, Y = 3.225 };
            var _changedSpatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key2, ObjectKeyValue = new TestCacheObject() };
            var _addOrUpdateExisting = _cache.AddOrUpdateExisting(_spatialItem.ObjectKeyValue, _changedSpatialItem, null, REGION);

            Assert.AreNotEqual(_addOrGetExisting, _addOrUpdateExisting);
            Assert.AreEqual(_changedSpatialItem, _addOrUpdateExisting);
        }
        [Test]
        public void AddOrUpdateExistingWhenAbsoluteExpirationTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 3.44, Y = 3.44 };
            var _object = new TestCacheObject();
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key, ObjectKeyValue = _object, SpatialValue = new object() };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _key2 = new Coordinate { X = 3.445, Y = 3.445 };
            var _changedSpatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key2, ObjectKeyValue = _object, SpatialValue = new object() };
            var _addOrUpdateExisting = _cache.AddOrUpdateExisting(_spatialItem.ObjectKeyValue, _changedSpatialItem, DateTimeOffset.UtcNow.AddMilliseconds(20));

            Assert.AreNotEqual(_addOrGetExisting, _addOrUpdateExisting);
            Assert.AreEqual(_changedSpatialItem, _addOrUpdateExisting);

            Thread.Sleep(100);

            Assert.IsNull(_cache.Get(_key));
            Assert.IsNull(_cache.Get(_key2));
        }

        [Test]
        public void UpdateTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 3.11, Y = 3.11 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key, ObjectKeyValue = new TestCacheObject() };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _key2 = new Coordinate { X = 3.115, Y = 3.115 };
            var _changedSpatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key2, ObjectKeyValue = new TestCacheObject() };
            var _value = _cache.Update(_spatialItem.ObjectKeyValue, _changedSpatialItem);

            Assert.AreNotEqual(_addOrGetExisting, _value);
            Assert.AreEqual(_changedSpatialItem, _value);
        }
        [Test]
        public void UpdateWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 3.22, Y = 3.22 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key, ObjectKeyValue = new TestCacheObject() };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _key2 = new Coordinate { X = 3.225, Y = 3.225 };
            var _changedSpatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key2, ObjectKeyValue = new TestCacheObject() };
            var _value = _cache.Update(_spatialItem.ObjectKeyValue, _changedSpatialItem, REGION);

            Assert.AreNotEqual(_addOrGetExisting, _value);
            Assert.AreEqual(_changedSpatialItem, _value);
        }
        [Test]
        public void UpdateWhenCacheItemPolicyTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 3.33, Y = 3.33 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key, ObjectKeyValue = new TestCacheObject() };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _key2 = new Coordinate { X = 3.335, Y = 3.335 };
            var _changedSpatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key2, ObjectKeyValue = new TestCacheObject() };
            var _value = _cache.Update(_spatialItem.ObjectKeyValue, _changedSpatialItem, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.UtcNow.AddMilliseconds(20) });

            Assert.AreNotEqual(_addOrGetExisting, _value);
            Assert.AreEqual(_changedSpatialItem, _value);

            Thread.Sleep(100);

            Assert.IsNull(_cache.Get(_key));
            Assert.IsNull(_cache.Get(_key2));
        }
        [Test]
        public void UpdateWhenAbsoluteExpirationTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 3.44, Y = 3.44 };
            var _object = new TestCacheObject();
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key, ObjectKeyValue = _object, SpatialValue = new object() };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _key2 = new Coordinate { X = 3.445, Y = 3.445 };
            var _changedSpatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key2, ObjectKeyValue = _object, SpatialValue = new object() };
            var _value = _cache.Update(_spatialItem.ObjectKeyValue, _changedSpatialItem, DateTimeOffset.UtcNow.AddMilliseconds(20));

            Assert.AreNotEqual(_addOrGetExisting, _value);
            Assert.AreEqual(_changedSpatialItem, _value);

            Thread.Sleep(100);

            Assert.IsNull(_cache.Get(_key));
            Assert.IsNull(_cache.Get(_key2));
        }

        [Test]
        public void RemoveTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 3.55, Y = 3.55 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            _cache.Remove(_key);

            var _actual = _cache.Get(_key);
            Assert.IsNull(_actual);
        }
        [Test]
        public void RemoveWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key = new Coordinate { X = 3.66, Y = 3.66 };
            var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            _cache.Remove(_key, REGION);

            var _actual = _cache.Get(_key);
            Assert.IsNull(_actual);
        }
        [Test]
        public void RemoveStressTest()
        {
            const int LOOPS1 = 3;
            const int LOOPS2 = 500;

            var _dictionary = new Dictionary<Coordinate, object>();
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();

            for (var _j = 1; _j <= LOOPS2; _j++)
            {
                var _key = new Coordinate { X = _j, Y = _j };
                var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key, SpatialValue = new object(), ObjectKeyValue = new TestCacheObject() };
                _cache.AddOrGetExisting(_key, _spatialItem);
                _dictionary.Add(_key, _spatialItem);
            }

            for (var _i = 0; _i < LOOPS1; _i++)
            {
                var _dateTimeOffSet = DateTimeOffset.UtcNow;

                for (var _j = 1; _j <= LOOPS2; _j++)
                {
                    var _key = new Coordinate { X = _j, Y = _j };
                    _cache.Remove(_key);
                }

                var _elapsedTime = (DateTimeOffset.UtcNow - _dateTimeOffSet).TotalMilliseconds;
                Assert.LessOrEqual(_elapsedTime, 50);
            }
        }

        [Test]
        public void QueryTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();

            var _key1 = new Coordinate { X = 40.708210, Y = -74.006074 };
            var _key2 = new Coordinate { X = 41.708210, Y = -73.006074 };
            var _key3 = new Coordinate { X = 42.708210, Y = -72.006074 };
            var _key4 = new Coordinate { X = 44.708210, Y = -71.006074 };

            var _spatialItem1 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key1 };
            var _spatialItem2 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key2 };
            var _spatialItem3 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key3 };
            var _spatialItem4 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key4 };

            _cache.AddOrGetExisting(_spatialItem1.SpatialKey, _spatialItem1);
            _cache.AddOrGetExisting(_spatialItem2.SpatialKey, _spatialItem2);
            _cache.AddOrGetExisting(_spatialItem3.SpatialKey, _spatialItem3);
            _cache.AddOrGetExisting(_spatialItem4.SpatialKey, _spatialItem4);

            var _key = new Coordinate { X = 40.055454, Y = -74.409822 };
            var _items = _cache.Query(_key, 50000);
            Assert.AreEqual(1, _items.Count());
        }
        [Test]
        public void QueryAreOrderedbyDistanceTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();

            var _key1 = new Coordinate { X = 0.0, Y = 0.0001 };
            var _key2 = new Coordinate { X = 0.0, Y = 0.0002 };
            var _key3 = new Coordinate { X = 0.0, Y = 0.0003 };
            var _key4 = new Coordinate { X = 0.0, Y = 0.00025 };

            var _spatialItem1 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key1, ObjectKeyValue = new TestCacheObject() };
            var _spatialItem2 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key2, ObjectKeyValue = new TestCacheObject() };
            var _spatialItem3 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key3, ObjectKeyValue = new TestCacheObject() };
            var _spatialItem4 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key4, ObjectKeyValue = new TestCacheObject() };

            _cache.AddOrGetExisting(_spatialItem1.SpatialKey, _spatialItem1);
            _cache.AddOrGetExisting(_spatialItem2.SpatialKey, _spatialItem2);
            _cache.AddOrGetExisting(_spatialItem3.SpatialKey, _spatialItem3);
            _cache.AddOrGetExisting(_spatialItem4.SpatialKey, _spatialItem4);

            var _key = new Coordinate();
            var _items = _cache.Query(_key, 500).ToList();

            Assert.AreEqual(4, _items.Count());
            Assert.AreEqual(_spatialItem1, _items.ElementAt(0));
            Assert.AreEqual(_spatialItem2, _items.ElementAt(1));
            Assert.AreEqual(_spatialItem4, _items.ElementAt(2));
            Assert.AreEqual(_spatialItem3, _items.ElementAt(3));
        }
        [Test]
        public void QueryAccuracyTest()
        {
            const double WITHIN_DISTANCE_IN_METERS = 500.00;

            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();

            var _key1 = new Coordinate { X = 55.655603, Y = 12.511976 };
            var _key2 = new Coordinate { X = 55.656550, Y = 12.512677 };
            var _key3 = new Coordinate { X = 55.656893, Y = 12.513004 };
            var _key4 = new Coordinate { X = 55.656231, Y = 12.512544 };
            var _key5 = new Coordinate { X = 55.655895, Y = 12.512169 };

            var _spatialItem1 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key1, ObjectKeyValue = new TestCacheObject() }; // 400 meters
            var _spatialItem2 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key2, ObjectKeyValue = new TestCacheObject() }; // 500 meters
            var _spatialItem3 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key3, ObjectKeyValue = new TestCacheObject() }; // 550 meters
            var _spatialItem4 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key4, ObjectKeyValue = new TestCacheObject() }; // 475 meters
            var _spatialItem5 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key5, ObjectKeyValue = new TestCacheObject() }; // 475 meters

            _cache.AddOrGetExisting(_spatialItem1.SpatialKey, _spatialItem1);
            _cache.AddOrGetExisting(_spatialItem2.SpatialKey, _spatialItem2);
            _cache.AddOrGetExisting(_spatialItem3.SpatialKey, _spatialItem3);
            _cache.AddOrGetExisting(_spatialItem4.SpatialKey, _spatialItem4);
            _cache.AddOrGetExisting(_spatialItem5.SpatialKey, _spatialItem5);

            var _key = new Coordinate { X = 55.652328, Y = 12.509476 };
            var _items = _cache.Query(_key, WITHIN_DISTANCE_IN_METERS).ToList();
            Assert.AreEqual(2, _items.Count());
            Assert.AreEqual(_spatialItem1, _items.ElementAt(0));
            Assert.AreEqual(_spatialItem5, _items.ElementAt(1));
        }
        [Test]
        public void QueryWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();

            var _key1 = new Coordinate { X = 40.708210, Y = -74.006074 };
            var _key2 = new Coordinate { X = 41.708210, Y = -73.006074 };
            var _key3 = new Coordinate { X = 42.708210, Y = -72.006074 };
            var _key4 = new Coordinate { X = 44.708210, Y = -71.006074 };

            var _spatialItem1 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key1 };
            var _spatialItem2 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key2 };
            var _spatialItem3 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key3 };
            var _spatialItem4 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key4 };

            _cache.AddOrGetExisting(_spatialItem1.SpatialKey, _spatialItem1, REGION);
            _cache.AddOrGetExisting(_spatialItem2.SpatialKey, _spatialItem2, REGION);
            _cache.AddOrGetExisting(_spatialItem3.SpatialKey, _spatialItem3, REGION);
            _cache.AddOrGetExisting(_spatialItem4.SpatialKey, _spatialItem4, REGION);

            var _key = new Coordinate { X = 40.055454, Y = -74.409822 };
            var _items = _cache.Query(_key, 50000, REGION);
            Assert.AreEqual(1, _items.Count());
        }
        [Test]
        public void QueryWhenEmptyTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();

            var _key1 = new Coordinate { X = 40.708210, Y = -74.006074 };
            var _key2 = new Coordinate { X = 41.708210, Y = -73.006074 };
            var _key3 = new Coordinate { X = 42.708210, Y = -72.006074 };
            var _key4 = new Coordinate { X = 44.708210, Y = -71.006074 };

            var _spatialItem1 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key1 };
            var _spatialItem2 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key2 };
            var _spatialItem3 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key3 };
            var _spatialItem4 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key4 };

            _cache.AddOrGetExisting(_spatialItem1.SpatialKey, _spatialItem1);
            _cache.AddOrGetExisting(_spatialItem2.SpatialKey, _spatialItem2);
            _cache.AddOrGetExisting(_spatialItem3.SpatialKey, _spatialItem3);
            _cache.AddOrGetExisting(_spatialItem4.SpatialKey, _spatialItem4);

            var _key = new Coordinate { X = 40.055454, Y = -74.409822 };
            var _items = _cache.Query(_key, 10000);
            Assert.IsEmpty(_items);
        }
        [Test]
        public void QueryStressTest()
        {
            const int LOOPS1 = 3;
            const int LOOPS2 = 500;

            var _dictionary = new Dictionary<Coordinate, object>();
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();

            for (var _j = 1; _j <= LOOPS2; _j++)
            {
                var _key = new Coordinate { X = _j, Y = _j };
                var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key, SpatialValue = new object(), ObjectKeyValue = new TestCacheObject() };
                _cache.AddOrGetExisting(_key, _spatialItem);
                _dictionary.Add(_key, _spatialItem);
            }

            for (var _i = 0; _i < LOOPS1; _i++)
            {
                var _dateTimeOffSet = DateTimeOffset.UtcNow;

                for (var _j = 1; _j <= LOOPS2; _j++)
                {
                    var _key = new Coordinate { X = _j, Y = _j };
                    _cache.Query(_key, 100000);
                }

                var _elapsedTime = (DateTimeOffset.UtcNow - _dateTimeOffSet).TotalMilliseconds;
                Assert.LessOrEqual(_elapsedTime, 25);
            }
        }
        [Test]
        public void QueryStressAsyncTest()
        {
            const int LOOPS1 = 3;
            const int LOOPS2 = 500;

            var _dictionary = new Dictionary<Coordinate, object>();
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();

            for (var _j = 1; _j <= LOOPS2; _j++)
            {
                var _key = new Coordinate { X = _j, Y = _j };
                var _spatialItem = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key, SpatialValue = new object(), ObjectKeyValue = new TestCacheObject() };
                _cache.AddOrGetExisting(_key, _spatialItem);
                _dictionary.Add(_key, _spatialItem);
            }

            for (var _i = 0; _i < LOOPS1; _i++)
            {
                var _dateTimeOffSet = DateTimeOffset.UtcNow;
                Parallel.ForEach(_dictionary, _x => _cache.Query(_x.Key, 100000));
                Assert.LessOrEqual((DateTimeOffset.UtcNow - _dateTimeOffSet).TotalMilliseconds, 50);
            }
        }

        [Test]
        public void IntersectTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();

            var _key1 = new Coordinate { X = 1.0, Y = 3.0 };
            var _key2 = new Coordinate { X = 1.5, Y = 1.5 };
            var _key3 = new Coordinate { X = 1.5, Y = 2.5 };
            var _key4 = new Coordinate { X = 6.0, Y = 2.5 };

            var _spatialItem1 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key1 };
            var _spatialItem2 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key2 };
            var _spatialItem3 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key3 };
            var _spatialItem4 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key4 };

            _cache.AddOrGetExisting(_spatialItem1.SpatialKey, _spatialItem1);
            _cache.AddOrGetExisting(_spatialItem2.SpatialKey, _spatialItem2);
            _cache.AddOrGetExisting(_spatialItem3.SpatialKey, _spatialItem3);
            _cache.AddOrGetExisting(_spatialItem3.SpatialKey, _spatialItem4);

            var _polygon = new Polygon(new LinearRing(new[] { new GeoAPI.Geometries.Coordinate(1, 4), new GeoAPI.Geometries.Coordinate(2, 4), new GeoAPI.Geometries.Coordinate(2, 1), new GeoAPI.Geometries.Coordinate(1, 1), new GeoAPI.Geometries.Coordinate(1, 4) }));
            var _items = _cache.Intersect(_polygon);

            Assert.AreEqual(3, _items.Count());
        }
        [Test]
        public void IntersectWhenRegionNameTest()
        {
            const string REGION = "TestRegion1";

            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();

            var _key1 = new Coordinate { X = 1.0, Y = 3.0 };
            var _key2 = new Coordinate { X = 1.5, Y = 1.5 };
            var _key3 = new Coordinate { X = 1.5, Y = 2.5 };
            var _key4 = new Coordinate { X = 6.0, Y = 2.5 };

            var _spatialItem1 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key1 };
            var _spatialItem2 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key2 };
            var _spatialItem3 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key3 };
            var _spatialItem4 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key4 };

            _cache.AddOrGetExisting(_spatialItem1.SpatialKey, _spatialItem1, REGION);
            _cache.AddOrGetExisting(_spatialItem2.SpatialKey, _spatialItem2, "TestRegion2");
            _cache.AddOrGetExisting(_spatialItem3.SpatialKey, _spatialItem3, "TestRegion3");
            _cache.AddOrGetExisting(_spatialItem4.SpatialKey, _spatialItem4, "TestRegion4");

            var _polygon = new Polygon(new LinearRing(new[] { new GeoAPI.Geometries.Coordinate(1, 4), new GeoAPI.Geometries.Coordinate(2, 4), new GeoAPI.Geometries.Coordinate(2, 1), new GeoAPI.Geometries.Coordinate(1, 1), new GeoAPI.Geometries.Coordinate(1, 4) }));
            var _items = _cache.Intersect(_polygon, REGION);

            Assert.AreEqual(1, _items.Count());
        }
        [Test]
        public void IntersectWhenEmptyTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();

            var _key1 = new Coordinate { X = 1.5, Y = 3.0 };
            var _key2 = new Coordinate { X = 3.0, Y = 1.5 };
            var _key3 = new Coordinate { X = 1.707210, Y = 1.006074 };
            var _key4 = new Coordinate { X = 4.708210, Y = 4.006074 };

            var _spatialItem1 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key1 };
            var _spatialItem2 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key2 };
            var _spatialItem3 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key3 };
            var _spatialItem4 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key4 };

            _cache.AddOrGetExisting(_spatialItem1.SpatialKey, _spatialItem1);
            _cache.AddOrGetExisting(_spatialItem2.SpatialKey, _spatialItem2);
            _cache.AddOrGetExisting(_spatialItem3.SpatialKey, _spatialItem3);
            _cache.AddOrGetExisting(_spatialItem4.SpatialKey, _spatialItem4);

            var _polygon = new Polygon(new LinearRing(new[] { new GeoAPI.Geometries.Coordinate(10, 40), new GeoAPI.Geometries.Coordinate(20, 40), new GeoAPI.Geometries.Coordinate(2, 1), new GeoAPI.Geometries.Coordinate(10, 10), new GeoAPI.Geometries.Coordinate(10, 40) }));
            var _items = _cache.Intersect(_polygon);

            Assert.AreEqual(0, _items.Count());
        }

        [Test]
        public void GetValuesTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key1 = new Coordinate { X = 7.11, Y = 7.11 };
            var _spatialItem1 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key1 };

            var _key2 = new Coordinate { X = 7.22, Y = 7.22 };
            var _spatialItem2 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key2 };

            _cache.AddOrGetExisting(_key1, _spatialItem1);
            _cache.AddOrGetExisting(_key2, _spatialItem2);

            var _list = _cache.GetValues(new[] { _key1, _key2 }).ToList();
            Assert.Contains(_key1, _list.Select(_x => _x.Key).ToList());
            Assert.Contains(_key2, _list.Select(_x => _x.Key).ToList());
        }
        [Test]
        public void GetValuesWhenValueNotParsedTest()
        {
            var _cache = new SpatialCache<Coordinate, object, TestCacheObject>();
            var _key1 = new Coordinate { X = 7.11, Y = 7.11 };
            var _spatialItem1 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key1 };

            var _key2 = new Coordinate { X = 7.22, Y = 7.22 };
            var _spatialItem2 = new SpatialCacheItem<Coordinate, object, TestCacheObject> { SpatialKey = _key2 };

            _cache.AddOrGetExisting(_key1, _spatialItem1);
            _cache.AddOrGetExisting(_key2, _spatialItem2);

            var _list = _cache.GetValues(new[] { _key1 }).ToList();
            Assert.Contains(_key1, _list.Select(_x => _x.Key).ToList());
            Assert.IsFalse(_list.Select(_x => _x.Key).Contains(_key2));
        }
 
        [Test]
        public void CreateCacheEntryChangeMonitorTest()
        {
            var _spatialCache = new SpatialCache<Coordinate, object, TestCacheObject>();
            Assert.Throws<NotSupportedException>(() => _spatialCache.CreateCacheEntryChangeMonitor(new List<Coordinate>()));
        }
        [Test]
        public void CreateCacheEntryChangeMonitorWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _spatialCache = new SpatialCache<Coordinate, object, TestCacheObject>();
            Assert.Throws<NotSupportedException>(() => _spatialCache.CreateCacheEntryChangeMonitor(new List<Coordinate>(), REGION));
        }

        [Test]
        public void GetEnumeratorTest()
        {
            var _enumerator = new SpatialCache<Coordinate, object, TestCacheObject>().GetEnumerator();
            Assert.AreEqual(_enumerator, new Dictionary<Coordinate, ISpatialCacheItem<Coordinate, object, TestCacheObject>>.Enumerator());
        }
    }
}