using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using RabbitCache.Caches;

namespace RabbitCache.Test.Caches
{
    [TestFixture]
    public class CollectionCacheTest
    {
        [Test]
        public void GetCountTest()
        {
            var _cache = new CollectionCache<string, object>();
            const string KEY = "Test";
            var _value = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(KEY, _value);
            Assert.AreEqual(_value, _addOrGetExisting);

            var _count = _cache.GetCount(KEY);
            Assert.AreEqual(1, _count);
        }
        [Test]
        public void GetCountWhenNoElementsTest()
        {
            var _cache = new CollectionCache<object, object>();

            var _count = _cache.GetCount(new object());
            Assert.AreEqual(0, _count);
        }
        [Test]
        public void GetCountWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new CollectionCache<object, object>();
            var _key = new object();
            var _value = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _value, REGION);
            Assert.AreEqual(_value, _addOrGetExisting);

            var _count = _cache.GetCount(_key, REGION);
            Assert.AreEqual(1, _count);
        }
        [Test]
        public void GetCountWhenRegionAndNoElementsTest()
        {
            const string REGION = "TestC";

            var _cache = new CollectionCache<object, object>();
            var _key = new object();
            var _keyValuePair = new KeyValuePair<object, object>(_key, new object());

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _keyValuePair, REGION);
            Assert.AreEqual(_keyValuePair, _addOrGetExisting);

            var _count = _cache.GetCount(_key);
            Assert.AreEqual(0, _count);
        }
        [Test]
        public void GetCountWhenRegionNameDoesNotExistsTest()
        {
            const string REGION = "TestA";
            const string REGION2 = "TestB";

            var _cache = new CollectionCache<object, object>();
            var _key = new object();
            var _keyValuePair = new KeyValuePair<object, object>(_key, new object());

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _keyValuePair, REGION);
            Assert.AreEqual(_keyValuePair, _addOrGetExisting);

            Assert.Throws<KeyNotFoundException>(() => _cache.GetCount(_key, REGION2));
        }

        [Test]
        public void ContainsTest()
        {
            var _cache = new CollectionCache<string, object>();
            const string KEY = "Test";
            var _value = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(KEY, _value);
            Assert.AreEqual(_value, _addOrGetExisting);

            var _contains = _cache.Contains(KEY, _value);
            Assert.IsTrue(_contains);
        }
        [Test]
        public void ContainsWhenFalseTest()
        {
            var _cache = new CollectionCache<string, object>();
            const string KEY = "Test";
            var _value = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(KEY, _value);
            Assert.AreEqual(_value, _addOrGetExisting);

            const string KEY2 = "Test2";
            var _contains = _cache.Contains(KEY2, _value);
            Assert.IsFalse(_contains);
        }
        [Test]
        public void ContainsWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new CollectionCache<object, object>();
            var _key = new object();
            var _value = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _value, REGION);
            Assert.AreEqual(_value, _addOrGetExisting);

            var _contains = _cache.Contains(_key, _value, REGION);
            Assert.IsTrue(_contains);
        }
        [Test]
        public void ContainsWhenRegionNameAndFalseTest()
        {
            const string REGION = "Test";

            var _cache = new CollectionCache<object, object>();
            var _key = new object();
            var _value = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _value, REGION);
            Assert.AreEqual(_value, _addOrGetExisting);

            var _contains = _cache.Contains(_key, _value);
            Assert.IsFalse(_contains);
        }
        [Test]
        public void ContainsWhenRegionNameDoesNotExistsTest()
        {
            const string REGION = "Test";
            const string REGION2 = "Test2";

            var _cache = new CollectionCache<object, object>();
            var _key = new object();
            var _value = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _value, REGION);
            Assert.AreEqual(_value, _addOrGetExisting);

            Assert.Throws<KeyNotFoundException>(() => _cache.Contains(_key, _value, REGION2));
        }

        [Test]
        public void GetTest()
        {
            var _cache = new CollectionCache<string, object>();
            const string KEY = "Test";
            var _value = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(KEY, _value);
            Assert.AreEqual(_value, _addOrGetExisting);

            var _actual = _cache.Get(KEY);
            Assert.AreEqual(_value, _actual.FirstOrDefault());
        }
        [Test]
        public void GetWhenNotExistsTest()
        {
            var _cache = new CollectionCache<object, object>();
            var _key = new object();

            var _actual = _cache.Get(_key);
            Assert.IsEmpty(_actual);
        }
        [Test]
        public void GetWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new CollectionCache<object, object>();
            var _key = new object();
            var _value = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _value, REGION);
            Assert.AreEqual(_value, _addOrGetExisting);

            var _actual = _cache.Get(_key, REGION);
            Assert.AreEqual(_value, _actual.FirstOrDefault());
        }
        [Test]
        public void GetWhenRegionNameAndNotExistsTest()
        {
            const string REGION = "Test2.44";

            var _cache = new CollectionCache<object, object>();
            var _key = new object();
            var _value = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _value, REGION);
            Assert.AreEqual(_value, _addOrGetExisting);

            var _key2 = new object();
            var _actual = _cache.Get(_key2, REGION);
            Assert.IsEmpty(_actual);
        }
        [Test]
        public void GetWhenRegionNameDoesNotExistsTest()
        {
            const string REGION = "Test";
            const string REGION2 = "Test2";

            var _cache = new CollectionCache<object, object>();
            var _key = new object();
            var _keyValuePair = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _keyValuePair, REGION);
            Assert.AreEqual(_keyValuePair, _addOrGetExisting);

            Assert.Throws<KeyNotFoundException>(() => _cache.Get(_key, REGION2));
        }
        [Test]
        public void GetStressTest()
        {
            const int LOOPS1 = 3;
            const int LOOPS2 = 500;

            var _dictionary = new Dictionary<string, object>();
            var _cache = new CollectionCache<string, object>();

            for (var _j = 1; _j <= LOOPS2; _j++)
            {
                var _key = "Test" + _j;
                var _keyValuePair = new object();
                _cache.AddOrGetExisting(_key, _keyValuePair);
                _dictionary.Add(_key, _keyValuePair);
            }

            for (var _i = 0; _i < LOOPS1; _i++)
            {
                var _dateTimeOffSet = DateTimeOffset.UtcNow;

                for (var _j = 1; _j <= LOOPS2; _j++)
                {
                    var _key = _dictionary.Where(_x => _x.Value == _dictionary["Test" + _j]).Select(_x => _x.Key).FirstOrDefault();
                    _cache.Get(_key);
                }

                var _elapsedTime = (DateTimeOffset.UtcNow - _dateTimeOffSet).TotalMilliseconds;
                Assert.LessOrEqual(_elapsedTime, 150);
            }
        }

        [Test]
        public void AddOrGetExistingTest()
        {
            var _cache = new CollectionCache<string, object>();
            const string KEY = "Test";
            var _value = new object();

            var _addOrGetExisting1 = _cache.AddOrGetExisting(KEY, _value);
            Assert.AreEqual(_value, _addOrGetExisting1);

            var _addOrGetExisting2 = _cache.AddOrGetExisting(KEY, _value);
            Assert.AreEqual(_value, _addOrGetExisting2);
        }
        [Test]
        public void AddOrGetExistingWhenNotExistsTest()
        {
            var _cache = new CollectionCache<string, object>();
            const string KEY = "Test";

            var _value = new KeyValuePair<string, object>(KEY, new object());
            var _actual = _cache.AddOrGetExisting(KEY, _value);

            Assert.AreEqual(_value, _actual);
        }
        [Test]
        public void AddOrGetExistingWithObjectTest()
        {
            var _cache = new CollectionCache<object, object>();
            var _key = typeof(object);
            var _value = typeof(object);

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _value, DateTimeOffset.UtcNow.AddMilliseconds(500));
            Assert.AreEqual(_value, _addOrGetExisting);
        }
        [Test]
        public void AddOrGetExistingWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new CollectionCache<object, object>();
            var _key = new object();
            var _value = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _value, REGION);
            Assert.AreEqual(_value, _addOrGetExisting);
        }
        [Test]
        public void AddOrGetExistingWhenCacheItemPolicyTest()
        {
            var _cache = new CollectionCache<object, object>();
            var _key = new object();
            var _value = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _value, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.UtcNow.AddMilliseconds(20) });
            Assert.AreEqual(_value, _addOrGetExisting);

            Thread.Sleep(80);

            var _actual = _cache.Get(_key);
            Assert.IsEmpty(_actual);
        }
        [Test]
        public void AddOrGetExistingWhenAbsoluteExpirationTest()
        {
            var _cache = new CollectionCache<string, object>();
            const string KEY = "Test";
            var _value = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(KEY, _value, DateTimeOffset.UtcNow.AddMilliseconds(20));
            Assert.AreEqual(_value, _addOrGetExisting);

            Thread.Sleep(80);

            var _actual = _cache.Get(KEY);
            Assert.IsEmpty(_actual);
        }
        [Test]
        public void AddOrGetExistingStressTest()
        {
            const int LOOPS1 = 3;
            const int LOOPS2 = 500;

            for (var _i = 0; _i < LOOPS1; _i++)
            {
                var _cache = new CollectionCache<string, object>();
                var _dateTimeOffSet = DateTimeOffset.UtcNow;

                for (var _j = 1; _j <= LOOPS2; _j++)
                {
                    var _key = "Test" + _j;
                    var _value = new object();
                    _cache.AddOrGetExisting(_key, _value);
                }

                var _elapsedTime = (DateTimeOffset.UtcNow - _dateTimeOffSet).TotalMilliseconds;
                Assert.LessOrEqual(_elapsedTime, 30);
            }
        }
        [Test]
        public void AddOrGetExistingAsynctressTest()
        {
            const int LOOPS1 = 3;
            const int LOOPS2 = 500;

            for (var _i = 0; _i < LOOPS1; _i++)
            {
                var _dictionary = new Dictionary<string, object>();
                var _cache = new CollectionCache<string, object>();

                for (var _j = 1; _j <= LOOPS2; _j++)
                {
                    var _key = "Test" + _j;
                    var _value = new object();
                    _dictionary.Add(_key, _value);
                }

                var _dateTimeOffSet = DateTimeOffset.UtcNow;
                Parallel.ForEach(_dictionary, _x => _cache.AddOrGetExisting(_x.Key, _x.Value));

                var _elapsedTime = (DateTimeOffset.UtcNow - _dateTimeOffSet).TotalMilliseconds;
                Assert.LessOrEqual(_elapsedTime, 200);
            }
        }
        [Test]
        public void AddOrGetExistingAsynctressWhenFastTimeoutTest()
        {
            const int LOOPS1 = 3;
            const int LOOPS2 = 500;

            for (var _i = 0; _i < LOOPS1; _i++)
            {
                var _dictionary = new Dictionary<object, object>();
                var _cache = new CollectionCache<object, object>();

                for (var _j = 1; _j <= LOOPS2; _j++)
                {
                    var _key = new object();
                    var _value = new object();
                    _dictionary.Add(_key, _value);
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
            var _cache = new CollectionCache<string, object>();
            const string KEY = "Test";
            var _value1 = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(KEY, _value1);
            Assert.AreEqual(_value1, _addOrGetExisting);

            var _value2 = new KeyValuePair<string, object>(KEY, new object());
            var _value = _cache.AddOrUpdateExisting(KEY, _value2);

            Assert.AreNotEqual(_addOrGetExisting, _value);
            Assert.AreEqual(_value2, _value);
        }
        [Test]
        public void AddOrUpdateExistingWhenNotExistsTest()
        {
            var _cache = new CollectionCache<string, object>();
            const string KEY = "Test";

            var _value = new KeyValuePair<string, object>(KEY, new object());
            var _actual = _cache.AddOrUpdateExisting(KEY, _value);

            Assert.AreEqual(_value, _actual);
        }
        [Test]
        public void AddOrUpdateExistingWithObjectTest()
        {
            var _cache = new CollectionCache<object, object>();
            var _key = typeof(object);
            var _value = typeof(object);

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _value, DateTimeOffset.UtcNow.AddMilliseconds(500));
            Assert.AreEqual(_value, _addOrGetExisting);

            var _value2 = new KeyValuePair<object, object>(_key, new object());
            var _actual = _cache.AddOrUpdateExisting(_key, _value2);

            Assert.AreEqual(_actual, _value2);
        }
        [Test]
        public void AddOrUpdateExistingWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new CollectionCache<object, object>();
            var _key = new object();
            var _value1 = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _value1, REGION);
            Assert.AreEqual(_value1, _addOrGetExisting);

            var _value2 = new KeyValuePair<object, object>(_key, new object());
            var _value = _cache.AddOrUpdateExisting(_key, _value2, null, REGION);

            Assert.AreNotEqual(_addOrGetExisting, _value);
            Assert.AreEqual(_value2, _value);
        }
        [Test]
        public void AddOrUpdateExistingWhenAbsoluteExpirationTest()
        {
            var _cache = new CollectionCache<string, object>();
            const string KEY = "Test";
            var _value1 = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(KEY, _value1);
            Assert.AreEqual(_value1, _addOrGetExisting);

            var _value2 = new object();
            var _value = _cache.AddOrUpdateExisting(KEY, _value2, DateTimeOffset.UtcNow.AddMilliseconds(20));

            Assert.AreNotEqual(_addOrGetExisting, _value);
            Assert.AreEqual(_value2, _value);

            Thread.Sleep(80);

            Assert.IsNull(_cache.Get(KEY).FirstOrDefault(_x => _x == _value));
        }

        [Test]
        public void UpdateTest()
        {
            var _cache = new CollectionCache<string, object>();
            const string KEY = "Test";
            var _value1 =new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(KEY, _value1);
            Assert.AreEqual(_value1, _addOrGetExisting);

            var _value2 = new KeyValuePair<string, object>(KEY, new object());
            var _value = _cache.Update(KEY, _value2);

            Assert.AreNotEqual(_addOrGetExisting, _value);
            Assert.AreEqual(_value2, _value);
        }
        [Test]
        public void UpdateWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new CollectionCache<object, object>();
            var _key = new object();
            var _value1 = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _value1, REGION);
            Assert.AreEqual(_value1, _addOrGetExisting);

            var _value2 = new KeyValuePair<object, object>(_key, new object());
            var _value = _cache.Update(_key, _value2, REGION);

            Assert.AreNotEqual(_addOrGetExisting, _value);
            Assert.AreEqual(_value2, _value);
        }
        [Test]
        public void UpdateWhenCacheItemPolicyTest()
        {
            var _cache = new CollectionCache<string, object>();
            const string KEY = "Test";
            var _value1 = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(KEY, _value1);
            Assert.AreEqual(_value1, _addOrGetExisting);

            var _value2 = new object();
            var _value = _cache.Update(KEY, _value2, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.UtcNow.AddMilliseconds(20) });

            Assert.AreNotEqual(_addOrGetExisting, _value);
            Assert.AreEqual(_value2, _value);

            Thread.Sleep(50);

            Assert.IsNull(_cache.Get(KEY).FirstOrDefault(_x => _x == _value));
        }
        [Test]
        public void UpdateWhenAbsoluteExpirationTest()
        {
            var _cache = new CollectionCache<string, object>();
            const string KEY = "Test";
            var _value1 = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(KEY, _value1);
            Assert.AreEqual(_value1, _addOrGetExisting);

            var _value2 = new object();
            var _value = _cache.Update(KEY, _value2, DateTimeOffset.UtcNow.AddMilliseconds(20));

            Assert.AreNotEqual(_addOrGetExisting, _value);
            Assert.AreEqual(_value2, _value);

            Thread.Sleep(80);

            Assert.IsNull(_cache.Get(KEY).FirstOrDefault(_x => _x == _value));
        }
        [Test]
        public void UpdateStressTest()
        {
            const int LOOPS1 = 3;
            const int LOOPS2 = 500;

            var _dictionary = new Dictionary<string, object>();
            var _cache = new CollectionCache<string, object>();

            for (var _j = 1; _j <= LOOPS2; _j++)
            {
                var _key = "Test" + _j;
                var _value = new object();
                _cache.AddOrGetExisting(_key, _value);
                _dictionary.Add(_key, _value);
            }

            for (var _i = 0; _i < LOOPS1; _i++)
            {
                var _dateTimeOffSet = DateTimeOffset.UtcNow;

                for (var _j = 1; _j <= LOOPS2; _j++)
                {
                    var _key = _dictionary.Where(_x => _x.Value == _dictionary["Test" + _j]).Select(_x => _x.Key).FirstOrDefault();
                    var _value = new object();
                    _cache.Update(_key, _value);
                }

                var _elapsedTime = (DateTimeOffset.UtcNow - _dateTimeOffSet).TotalMilliseconds;
                Assert.LessOrEqual(_elapsedTime, 150);
            }
        }
        [Test]
        public void UpdateStressAsyncTest()
        {
            const int LOOPS1 = 3;
            const int LOOPS2 = 500;

            var _dictionary = new Dictionary<string, object>();
            var _cache = new CollectionCache<string, object>();

            for (var _j = 1; _j <= LOOPS2; _j++)
            {
                var _key = "Test" + _j;
                var _value = new object();
                _cache.AddOrGetExisting(_key, _value);
                _dictionary.Add(_key, _value);
            }

            for (var _i = 0; _i < LOOPS1; _i++)
            {
                var _dateTimeOffSet = DateTimeOffset.UtcNow;
                Parallel.ForEach(_dictionary, _x =>
                {
                    var _value = new object();
                    _cache.Update(_x.Key, _value);
                });

                var _elapsedTime = (DateTimeOffset.UtcNow - _dateTimeOffSet).TotalMilliseconds;
                Assert.LessOrEqual(_elapsedTime, 150);
            }
        }
        
        [Test]
        public void RemoveTest()
        {
            var _cache = new CollectionCache<string, object>();
            const string KEY = "Test";
            var _value = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(KEY, _value);
            Assert.AreEqual(_value, _addOrGetExisting);

            _cache.Remove(KEY, _value);

            var _actual = _cache.Get(KEY);
            Assert.IsEmpty(_actual);
        }
        [Test]
        public void RemoveWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _cache = new CollectionCache<object, object>();
            var _key = new object();
            var _value = new object();

            var _addOrGetExisting = _cache.AddOrGetExisting(_key, _value, REGION);
            Assert.AreEqual(_value, _addOrGetExisting);

            _cache.Remove(_key, _value, REGION);

            var _actual = _cache.Get(_key);
            Assert.IsEmpty(_actual);
        }
        [Test]
        public void RemoveStressTest()
        {
            const int LOOPS1 = 3;
            const int LOOPS2 = 500;

            var _cache = new CollectionCache<string, string>();
            var _dictionary = new Dictionary<string, string>();

            for (var _j = 1; _j <= LOOPS2; _j++)
            {
                var _key = "Test" + _j;
                var _value = _j.ToString();
                _cache.AddOrGetExisting(_key, _value);
                _dictionary.Add(_key, _value);
            }

            for (var _i = 0; _i < LOOPS1; _i++)
            {
                var _dateTimeOffSet = DateTimeOffset.UtcNow;

                for (var _j = 1; _j <= LOOPS2; _j++)
                {
                    var _key = _dictionary.Where(_x => _x.Value == _dictionary["Test" + _j]).Select(_x => _x.Key).FirstOrDefault();
                    var _value = _dictionary["Test" + _j].Where(_x => _x.ToString() == _j.ToString()).ToString();

                    _cache.Remove(_key, _value);
                }

                var _elapsedTime = (DateTimeOffset.UtcNow - _dateTimeOffSet).TotalMilliseconds;
                Assert.LessOrEqual(_elapsedTime, 100);
            }
        }
        
        [Test]
        public void GetValuesTest()
        {
            var _cache = new CollectionCache<string, object>();
            
            const string KEY1 = "Test1";
            var _value1 = new object();

            const string KEY2 = "Test2";
            var _value2 = new object();

            _cache.AddOrGetExisting(KEY1, _value1);
            _cache.AddOrGetExisting(KEY2, _value2);

            var _list = _cache.GetValues(new[] { KEY1, KEY2 }).ToList();
            Assert.Contains(KEY1, _list.Select(_x => _x.Key).ToList());
            Assert.Contains(KEY2, _list.Select(_x => _x.Key).ToList());
        }
        [Test]
        public void GetValuesWhenValueNotParsedTest()
        {
            var _cache = new CollectionCache<string, object>();

            const string KEY1 = "Test1";
            var _value1 = new object();

            const string KEY2 = "Test2";
            var _value2 = new object();

            _cache.AddOrGetExisting(KEY1, _value1);
            _cache.AddOrGetExisting(KEY2, _value2);

            var _list = _cache.GetValues(new[] { KEY1 }).ToList();
            Assert.Contains(KEY1, _list.Select(_x => _x.Key).ToList());
            Assert.IsFalse(_list.Select(_x => _x.Key).Contains(KEY2));
        }
 
        [Test]
        public void CreateCacheEntryChangeMonitorTest()
        {
            var _spatialCache = new CollectionCache<object, object>();
            Assert.Throws<NotSupportedException>(() => _spatialCache.CreateCacheEntryChangeMonitor(new List<object>()));
        }
        [Test]
        public void CreateCacheEntryChangeMonitorWhenRegionNameTest()
        {
            const string REGION = "Test";

            var _spatialCache = new CollectionCache<object, object>();
            Assert.Throws<NotSupportedException>(() => _spatialCache.CreateCacheEntryChangeMonitor(new List<object>(), REGION));
        }

        [Test]
        public void GetEnumeratorTest()
        {
            var _enumerator = new CollectionCache<object, object>().GetEnumerator();
            Assert.AreEqual(_enumerator, new Dictionary<object, IList<object>>.Enumerator());
        }
    }
}