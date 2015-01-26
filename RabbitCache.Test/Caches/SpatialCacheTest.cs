using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using GeoAPI.Geometries;
using NUnit.Framework;
using RabbitCache.Caches;
using RabbitCache.Caches.Entities;
using RabbitCache.Caches.Entities.Interfaces;

namespace RabbitCache.Test.Caches
{
    [TestFixture]
    public class SpatialCacheTest
    {
        [Test]
        public void GetCountTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(1.11, 1.11);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _count = _cache.GetCount();
            Assert.AreEqual(1, _count);
        }
        [Test]
        public void GetCountWhenNoElementsTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();

            var _count = _cache.GetCount();
            Assert.AreEqual(0, _count);
        }
        [Test]
        public void GetCountWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(1.22, 1.22);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _count = _cache.GetCount(REGION);
            Assert.AreEqual(1, _count);
        }
        [Test]
        public void GetCountWhenRegionAndNoElementsTest()
        {
            const string REGION = "Test";

            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(1.33, 1.33);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

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

            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(1.44, 1.44);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            Assert.Throws<KeyNotFoundException>(() => _cache.GetCount(REGION2));
        }

        [Test]
        public void ContainsTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(1.55, 1.55);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _contains = _cache.Contains(_key);
            Assert.IsTrue(_contains);
        }
        [Test]
        public void ContainsWhenFalseTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(1.66, 1.66);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _key2 = new Coordinate(1.664, 1.664);
            var _contains = _cache.Contains(_key2);
            Assert.IsFalse(_contains);
        }
        [Test]
        public void ContainsWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(1.77, 1.77);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _contains = _cache.Contains(_key, REGION);
            Assert.IsTrue(_contains);
        }
        [Test]
        public void ContainsWhenRegionNameAndFalseTest()
        {
            const string REGION = "Test";

            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(1.88, 1.88);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

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

            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(1.99, 1.99);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _contains = _cache.Contains(_key, REGION2);
            Assert.IsFalse(_contains);
        }

        [Test]
        public void GetTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(2.11, 2.11);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _actual = _cache.Get(_key);
            Assert.AreEqual(_spatialItem, _actual);
        }
        [Test]
        public void GetWhenNotExistsTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(2.22, 2.22);

            var _actual = _cache.Get(_key);
            Assert.IsNull(_actual);
        }
        [Test]
        public void GetWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(2.33, 2.33);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _actual = _cache.Get(_key, REGION);
            Assert.AreEqual(_spatialItem, _actual);
        }
        [Test]
        public void GetWhenRegionNameAndNotExistsTest()
        {
            const string REGION = "Test2.44";

            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(2.44, 2.44);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _key2 = new Coordinate(12.445, 12.445);
            var _actual = _cache.Get(_key2, REGION);
            Assert.IsNull(_actual);
        }
        [Test]
        public void GetWhenRegionNameDoesNotExistsTest()
        {
            const string REGION = "Test";
            const string REGION2 = "Test2";

            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(2.55, 2.55);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

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
            var _cache = new SpatialCache<Coordinate, object, object>();

            for (var _j = 1; _j <= LOOPS2; _j++)
            {
                var _key = new Coordinate(_j, _j);
                var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key, SpatialValue = new object(), ObjectKeyValue = new object() };
                _cache.AddOrGetExisting(_key, _spatialItem);
                _dictionary.Add(_key, _spatialItem);
            }

            for (var _i = 0; _i < LOOPS1; _i++)
            {
                var _dateTimeOffSet = DateTimeOffset.UtcNow;

                for (var _j = 1; _j <= LOOPS2; _j++)
                {
                    var _key = new Coordinate(_j, _j);
                    _cache.Get(_key);
                }

                var _elapsedTime = (DateTimeOffset.UtcNow - _dateTimeOffSet).TotalMilliseconds;
                Assert.LessOrEqual(_elapsedTime, 30);
            }
        }
 
        [Test]
        public void AddOrGetExistingTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(2.66, 2.66);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);
        }
        [Test]
        public void AddOrGetExistingWhenNotExistsTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(2.66, 2.66);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);
        }
        [Test]
        public void AddOrGetExistingWithObjectTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(2.66, 2.66);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting((object)_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);
        }
        [Test]
        public void AddOrGetExistingWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(2.77, 2.77);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);
        }
        [Test]
        public void AddOrGetExistingWhenCacheItemPolicyTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(2.88, 2.88);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.UtcNow.AddMilliseconds(20) });
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            Thread.Sleep(80);

            var _actual = _cache.Get(_key);
            Assert.IsNull(_actual);
        }
        [Test]
        public void AddOrGetExistingWhenAbsoluteExpirationTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(2.99, 2.99);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

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
                var _cache = new SpatialCache<Coordinate, object, object>();
                var _dateTimeOffSet = DateTimeOffset.UtcNow;

                for (var _j = 1; _j <= LOOPS2; _j++)
                {
                    var _key = new Coordinate(_j, _j);
                    var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key, SpatialValue = new object(), ObjectKeyValue = new object() };
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
                var _dictionary = new Dictionary<Coordinate, SpatialCacheItem<Coordinate, object, object>>();
                var _cache = new SpatialCache<Coordinate, object, object>();

                for (var _j = 1; _j <= LOOPS2; _j++)
                {
                    var _key = new Coordinate(_j, _j);
                    var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key, SpatialValue = new object(), ObjectKeyValue = new object() };
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
                var _dictionary = new Dictionary<Coordinate, SpatialCacheItem<Coordinate, object, object>>();
                var _cache = new SpatialCache<Coordinate, object, object>();

                for (var _j = 1; _j <= LOOPS2; _j++)
                {
                    var _key = new Coordinate(_j, _j);
                    var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key, SpatialValue = new object(), ObjectKeyValue = new object() };
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
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(3.11, 3.11);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key, ObjectKeyValue = new object() };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _key2 = new Coordinate(3.115, 3.115);
            var _changedSpatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key2, ObjectKeyValue = new object() };
            var _addOrUpdateExisting = _cache.AddOrUpdateExisting(_spatialItem.ObjectKeyValue, _changedSpatialItem);

            Assert.AreNotEqual(_addOrGetExisting, _addOrUpdateExisting);
            Assert.AreEqual(_changedSpatialItem, _addOrUpdateExisting);
        }
        [Test]
        public void AddOrUpdateExistingWhenNotExistsTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(2.66, 2.66);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key, ObjectKeyValue = new object()};

            var _addOrUpdateExisting = _cache.AddOrUpdateExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrUpdateExisting);
        }
        [Test]
        public void AddOrUpdateExistingWithObjectTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(2.66, 2.66);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key, ObjectKeyValue = new object() };

            var _addOrUpdateExisting = _cache.AddOrUpdateExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrUpdateExisting);
        }
        [Test]
        public void AddOrUpdateExistingWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(3.22, 3.22);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key, ObjectKeyValue = new object() };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _key2 = new Coordinate(3.225, 3.225);
            var _changedSpatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key2, ObjectKeyValue = new object() };
            var _addOrUpdateExisting = _cache.AddOrUpdateExisting(_spatialItem.ObjectKeyValue, _changedSpatialItem, null, REGION);

            Assert.AreNotEqual(_addOrGetExisting, _addOrUpdateExisting);
            Assert.AreEqual(_changedSpatialItem, _addOrUpdateExisting);
        }
        [Test]
        public void AddOrUpdateExistingWhenAbsoluteExpirationTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(3.44, 3.44);
            var _object = new object();
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key, ObjectKeyValue = _object, SpatialValue = new object() };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _key2 = new Coordinate(3.445, 3.445);
            var _changedSpatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key2, ObjectKeyValue = _object, SpatialValue = new object() };
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
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(3.11, 3.11);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key, ObjectKeyValue = new object() };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _key2 = new Coordinate(3.115, 3.115);
            var _changedSpatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key2, ObjectKeyValue = new object() };
            var _value = _cache.Update(_spatialItem.ObjectKeyValue, _changedSpatialItem);

            Assert.AreNotEqual(_addOrGetExisting, _value);
            Assert.AreEqual(_changedSpatialItem, _value);
        }
        [Test]
        public void UpdateWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(3.22, 3.22);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key, ObjectKeyValue = new object() };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem, REGION);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _key2 = new Coordinate(3.225, 3.225);
            var _changedSpatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key2, ObjectKeyValue = new object() };
            var _value = _cache.Update(_spatialItem.ObjectKeyValue, _changedSpatialItem, REGION);

            Assert.AreNotEqual(_addOrGetExisting, _value);
            Assert.AreEqual(_changedSpatialItem, _value);
        }
        [Test]
        public void UpdateWhenCacheItemPolicyTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(3.33, 3.33);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key, ObjectKeyValue = new object() };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _key2 = new Coordinate(3.335, 3.335);
            var _changedSpatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key2, ObjectKeyValue = new object() };
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
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(3.44, 3.44);
            var _object = new object();
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key, ObjectKeyValue = _object, SpatialValue = new object() };

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _spatialItem);
            Assert.AreEqual(_spatialItem, _addOrGetExisting);

            var _key2 = new Coordinate(3.445, 3.445);
            var _changedSpatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key2, ObjectKeyValue = _object, SpatialValue = new object() };
            var _value = _cache.Update(_spatialItem.ObjectKeyValue, _changedSpatialItem, DateTimeOffset.UtcNow.AddMilliseconds(20));

            Assert.AreNotEqual(_addOrGetExisting, _value);
            Assert.AreEqual(_changedSpatialItem, _value);

            Thread.Sleep(100);

            Assert.IsNull(_cache.Get(_key));
            Assert.IsNull(_cache.Get(_key2));
        }
        [Test]
        public void UpdateStressTest()
        {
            const int LOOPS1 = 3;
            const int LOOPS2 = 200;

            var _dictionary = new Dictionary<Coordinate, object>();
            var _cache = new SpatialCache<Coordinate, object, object>();

            for (var _j = 1; _j <= LOOPS2; _j++)
            {
                var _key = new Coordinate(_j, _j);
                var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key, SpatialValue = new object(), ObjectKeyValue = new object() };
                _cache.AddOrGetExisting(_key, _spatialItem);
                _dictionary.Add(_key, _spatialItem);
            }

            for (var _i = 0; _i < LOOPS1; _i++)
            {
                var _dateTimeOffSet = DateTimeOffset.UtcNow;

                for (var _j = 1; _j <= LOOPS2; _j++)
                {
                    var _key = new Coordinate(_j, _j);
                    var _newKey = new Coordinate(_j + 0.1, _j + 0.1);
                    var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _newKey, SpatialValue = new object(), ObjectKeyValue = _dictionary[_key] };
                    _cache.Update(_key, _spatialItem);
                }

                var _elapsedTime = (DateTimeOffset.UtcNow - _dateTimeOffSet).TotalMilliseconds;
                Assert.LessOrEqual(_elapsedTime, 80);
            }
        }
        [Test]
        public void UpdateStressAsyncTest()
        {
            const int LOOPS1 = 3;
            const int LOOPS2 = 200;

            var _dictionary = new Dictionary<Coordinate, object>();
            var _cache = new SpatialCache<Coordinate, object, object>();

            for (var _j = 1; _j <= LOOPS2; _j++)
            {
                var _key = new Coordinate(_j, _j);
                var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key, SpatialValue = new object(), ObjectKeyValue = new object() };
                _cache.AddOrGetExisting(_key, _spatialItem);
                _dictionary.Add(_key, _spatialItem);
            }

            for (var _i = 0; _i < LOOPS1; _i++)
            {
                var _dateTimeOffSet = DateTimeOffset.UtcNow;
                Parallel.ForEach(_dictionary, _x =>
                {
                    var _newKey = new Coordinate(_x.Key.X + 0.1, _x.Key.Y + 0.1);
                    var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _newKey, SpatialValue = new object(), ObjectKeyValue = _dictionary[_x.Key] };
                    _cache.Update(_x.Key, _spatialItem);
                });

                var _elapsedTime = (DateTimeOffset.UtcNow - _dateTimeOffSet).TotalMilliseconds;
                Assert.LessOrEqual(_elapsedTime, 80);
            }
        }
        
        [Test]
        public void RemoveTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(3.55, 3.55);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

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

            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key = new Coordinate(3.66, 3.66);
            var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key };

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
            var _cache = new SpatialCache<Coordinate, object, object>();

            for (var _j = 1; _j <= LOOPS2; _j++)
            {
                var _key = new Coordinate(_j, _j);
                var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key, SpatialValue = new object(), ObjectKeyValue = new object() };
                _cache.AddOrGetExisting(_key, _spatialItem);
                _dictionary.Add(_key, _spatialItem);
            }

            for (var _i = 0; _i < LOOPS1; _i++)
            {
                var _dateTimeOffSet = DateTimeOffset.UtcNow;

                for (var _j = 1; _j <= LOOPS2; _j++)
                {
                    var _key = new Coordinate(_j, _j);
                    _cache.Remove(_key);
                }

                var _elapsedTime = (DateTimeOffset.UtcNow - _dateTimeOffSet).TotalMilliseconds;
                Assert.LessOrEqual(_elapsedTime, 50);
            }
        }

        [Test]
        public void QueryTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();

            var _spatialItem1 = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = new Coordinate(40.708210, -74.006074) };
            var _spatialItem2 = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = new Coordinate(41.708210, -73.006074) };
            var _spatialItem3 = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = new Coordinate(42.708210, -72.006074) };
            var _spatialItem4 = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = new Coordinate(44.708210, -71.006074) };

            _cache.AddOrGetExisting(_spatialItem1.SpatialKey, _spatialItem1);
            _cache.AddOrGetExisting(_spatialItem2.SpatialKey, _spatialItem2);
            _cache.AddOrGetExisting(_spatialItem3.SpatialKey, _spatialItem3);
            _cache.AddOrGetExisting(_spatialItem4.SpatialKey, _spatialItem4);

            var _items = _cache.Query(new Coordinate(40.055454, -74.409822), 50000);
            Assert.AreEqual(1, _items.Count());
        }
        [Test]
        public void QueryAreOrderedbyDistanceTest()
        {
            var _cache = new SpatialCache<Coordinate, object, string>();

            var _spatialItem1 = new SpatialCacheItem<Coordinate, object, string> { SpatialKey = new Coordinate(0.0, 0.0001), ObjectKeyValue = "item 1" };
            var _spatialItem2 = new SpatialCacheItem<Coordinate, object, string> { SpatialKey = new Coordinate(0.0, 0.0002), ObjectKeyValue = "item 2" };
            var _spatialItem3 = new SpatialCacheItem<Coordinate, object, string> { SpatialKey = new Coordinate(0.0, 0.0003), ObjectKeyValue = "item 3" };
            var _spatialItem4 = new SpatialCacheItem<Coordinate, object, string> { SpatialKey = new Coordinate(0.0, 0.00025), ObjectKeyValue = "item 4" };

            _cache.AddOrGetExisting(_spatialItem1.SpatialKey, _spatialItem1);
            _cache.AddOrGetExisting(_spatialItem2.SpatialKey, _spatialItem2);
            _cache.AddOrGetExisting(_spatialItem3.SpatialKey, _spatialItem3);
            _cache.AddOrGetExisting(_spatialItem4.SpatialKey, _spatialItem4);

            var _items = _cache.Query(new Coordinate(0.0, 0.00029), 50000).ToList();
            Assert.AreEqual(4, _items.Count());
            Assert.AreEqual(_spatialItem3, _items.ElementAt(0), _items.ElementAt(0).ObjectKeyValue);
            Assert.AreEqual(_spatialItem4, _items.ElementAt(1), _items.ElementAt(1).ObjectKeyValue);
            Assert.AreEqual(_spatialItem2, _items.ElementAt(2), _items.ElementAt(2).ObjectKeyValue);
            Assert.AreEqual(_spatialItem1, _items.ElementAt(3), _items.ElementAt(3).ObjectKeyValue);
            
        }
        [Test]
        public void QueryAccuracyTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();

            var _spatialItem1 = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = new Coordinate(55.825199, 12.385061) };
            var _spatialItem2 = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = new Coordinate(55.823849, 12.375277) };
            var _spatialItem3 = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = new Coordinate(56.823849, 12.375277) };

            _cache.AddOrGetExisting(_spatialItem1.SpatialKey, _spatialItem1);
            _cache.AddOrGetExisting(_spatialItem2.SpatialKey, _spatialItem2);
            _cache.AddOrGetExisting(_spatialItem3.SpatialKey, _spatialItem3);

            var _items = _cache.Query(new Coordinate(55.822644, 12.369440), 800);
            Assert.AreEqual(1, _items.Count());

            var _items2 = _cache.Query(new Coordinate(55.822644, 12.369440), 2000);
            Assert.AreEqual(2, _items2.Count());
        }
        [Test]
        public void QueryWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new SpatialCache<Coordinate, object, object>();

            var _spatialItem1 = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = new Coordinate(40.708210, -74.006074) };
            var _spatialItem2 = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = new Coordinate(41.708210, -73.006074) };
            var _spatialItem3 = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = new Coordinate(42.708210, -72.006074) };
            var _spatialItem4 = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = new Coordinate(44.708210, -71.006074) };

            _cache.AddOrGetExisting(_spatialItem1.SpatialKey, _spatialItem1, REGION);
            _cache.AddOrGetExisting(_spatialItem2.SpatialKey, _spatialItem2, REGION);
            _cache.AddOrGetExisting(_spatialItem3.SpatialKey, _spatialItem3, REGION);
            _cache.AddOrGetExisting(_spatialItem4.SpatialKey, _spatialItem4, REGION);

            var _items = _cache.Query(new Coordinate(40.055454, -74.409822), 50000, REGION);
            Assert.AreEqual(1, _items.Count());
        }
        [Test]
        public void QueryWhenEmptyTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();

            var _spatialItem1 = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = new Coordinate(40.708210, -74.006074) };
            var _spatialItem2 = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = new Coordinate(41.708210, -73.006074) };
            var _spatialItem3 = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = new Coordinate(42.708210, -72.006074) };
            var _spatialItem4 = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = new Coordinate(44.708210, -71.006074) };

            _cache.AddOrGetExisting(_spatialItem1.SpatialKey, _spatialItem1);
            _cache.AddOrGetExisting(_spatialItem2.SpatialKey, _spatialItem2);
            _cache.AddOrGetExisting(_spatialItem3.SpatialKey, _spatialItem3);
            _cache.AddOrGetExisting(_spatialItem4.SpatialKey, _spatialItem4);

            var _items = _cache.Query(new Coordinate(40.055454, -74.409822), 10000);
            Assert.IsEmpty(_items);
        }
        [Test]
        public void QueryStressTest()
        {
            const int LOOPS1 = 3;
            const int LOOPS2 = 500;

            var _dictionary = new Dictionary<Coordinate, object>();
            var _cache = new SpatialCache<Coordinate, object, object>();

            for (var _j = 1; _j <= LOOPS2; _j++)
            {
                var _key = new Coordinate(_j, _j);
                var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key, SpatialValue = new object(), ObjectKeyValue = new object() };
                _cache.AddOrGetExisting(_key, _spatialItem);
                _dictionary.Add(_key, _spatialItem);
            }

            for (var _i = 0; _i < LOOPS1; _i++)
            {
                var _dateTimeOffSet = DateTimeOffset.UtcNow;

                for (var _j = 1; _j <= LOOPS2; _j++)
                {
                    var _key = new Coordinate(_j, _j);
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
            var _cache = new SpatialCache<Coordinate, object, object>();

            for (var _j = 1; _j <= LOOPS2; _j++)
            {
                var _key = new Coordinate(_j, _j);
                var _spatialItem = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key, SpatialValue = new object(), ObjectKeyValue = new object() };
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
        public void GetValuesTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key1 = new Coordinate(7.11, 7.11);
            var _spatialItem1 = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key1 };

            var _key2 = new Coordinate(7.22, 7.22);
            var _spatialItem2 = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key2 };

            _cache.AddOrGetExisting(_key1, _spatialItem1);
            _cache.AddOrGetExisting(_key2, _spatialItem2);

            var _list = _cache.GetValues(new[] { _key1, _key2 }).ToList();
            Assert.Contains(_key1, _list.Select(_x => _x.Key).ToList());
            Assert.Contains(_key2, _list.Select(_x => _x.Key).ToList());
        }
        [Test]
        public void GetValuesWhenValueNotParsedTest()
        {
            var _cache = new SpatialCache<Coordinate, object, object>();
            var _key1 = new Coordinate(7.11, 7.11);
            var _spatialItem1 = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key1 };

            var _key2 = new Coordinate(7.22, 7.22);
            var _spatialItem2 = new SpatialCacheItem<Coordinate, object, object> { SpatialKey = _key2 };

            _cache.AddOrGetExisting(_key1, _spatialItem1);
            _cache.AddOrGetExisting(_key2, _spatialItem2);

            var _list = _cache.GetValues(new[] { _key1 }).ToList();
            Assert.Contains(_key1, _list.Select(_x => _x.Key).ToList());
            Assert.IsFalse(_list.Select(_x => _x.Key).Contains(_key2));
        }
 
        [Test]
        public void CreateCacheEntryChangeMonitorTest()
        {
            var _spatialCache = new SpatialCache<Coordinate, object, object>();
            Assert.Throws<NotSupportedException>(() => _spatialCache.CreateCacheEntryChangeMonitor(new List<Coordinate>()));
        }
        [Test]
        public void CreateCacheEntryChangeMonitorWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _spatialCache = new SpatialCache<Coordinate, object, object>();
            Assert.Throws<NotSupportedException>(() => _spatialCache.CreateCacheEntryChangeMonitor(new List<Coordinate>(), REGION));
        }

        [Test]
        public void GetEnumeratorTest()
        {
            var _enumerator = new SpatialCache<Coordinate, object, object>().GetEnumerator();
            Assert.AreEqual(_enumerator, new Dictionary<Coordinate, ISpatialCacheItem<Coordinate, object, object>>.Enumerator());
        }
    }
}