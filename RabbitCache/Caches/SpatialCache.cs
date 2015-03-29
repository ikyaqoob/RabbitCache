using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.Index.Quadtree;
using RabbitCache.Caches.Entities;
using RabbitCache.Caches.Entities.Interfaces;
using RabbitCache.Caches.Interfaces;
using RabbitCache.Caches.Locking;
using RabbitCache.Extensions;

namespace RabbitCache.Caches
{
    public class SpatialCache<TSpatialKey, TSpatialValue, TObjectKeyValue> : BaseCache, ISpatialCache<TSpatialKey, TSpatialValue, TObjectKeyValue>
        where TSpatialKey : Coordinate
        where TSpatialValue : class
        where TObjectKeyValue : class
    {
        private readonly ConcurrentDictionary<string, List<ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue>>> _objectCaches;
        private readonly ConcurrentDictionary<string, Quadtree<ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue>>> _spatialCaches;

        public override DefaultCacheCapabilities DefaultCacheCapabilities
        {
            get { return DefaultCacheCapabilities.CacheRegions | DefaultCacheCapabilities.InMemoryProvider | DefaultCacheCapabilities.AbsoluteExpirations; }
        }

        public SpatialCache()
            : this(Guid.NewGuid().ToString("N"))
        {
        }
        public SpatialCache(string _name)
            : base(_name)
        {
            this._objectCaches = new ConcurrentDictionary<string, List<ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue>>>();
            this._spatialCaches = new ConcurrentDictionary<string, Quadtree<ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue>>>();

            this.AddRegion(BaseCache.DEFAULT_REGION);
        }

        public virtual long GetCount(string _regionName = null)
        {
            _regionName = _regionName ?? BaseCache.DEFAULT_REGION;

            var _spatialCache = this._spatialCaches[_regionName];
            var _syncLock = this._syncLocks[_regionName];

            using (new ReaderLock(_syncLock))
            {
                return _spatialCache.Count;
            }
        }
        public virtual bool Contains(TSpatialKey _key, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            return this.Get(_key, _regionName) != null;
        }
        public virtual object AddOrGetExisting(object _key, object _value, DateTimeOffset? _absoluteExpiration = null, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            if (_value == null)
                throw new ArgumentNullException("_value");

            var _realKey = (TSpatialKey)_key;
            var _realValue = (SpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue>)_value;

            return this.AddOrGetExisting(_realKey, _realValue, _absoluteExpiration, _regionName);
        }
        public virtual object AddOrUpdateExisting(object _key, object _value, DateTimeOffset? _absoluteExpiration = null, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            if (_value == null)
                throw new ArgumentNullException("_value");

            var _realValue = (SpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue>)_value;
            var _realKey = _realValue.ObjectKeyValue;

            return this.AddOrUpdateExisting(_realKey, _realValue, _absoluteExpiration, _regionName);
        }
        public virtual ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue> Get(TSpatialKey _key, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            _regionName = _regionName ?? BaseCache.DEFAULT_REGION;

            try
            {
                var _spatialCache = this._spatialCaches[_regionName];
                var _syncLock = this._syncLocks[_regionName];

                using (new ReaderLock(_syncLock))
                {
                    return this.Get(_spatialCache, _key);
                }
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
        public virtual ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue> Get(TObjectKeyValue _key, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            _regionName = _regionName ?? BaseCache.DEFAULT_REGION;

            var _objectCache = this._objectCaches[_regionName];
            var _syncLock = this._syncLocks[_regionName];

            using (new ReaderLock(_syncLock))
            {
                return _objectCache.FirstOrDefault(_x => _x.ObjectKeyValue == _key);
            }
        }
        public virtual ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue> AddOrGetExisting(TSpatialKey _key, ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue> _value, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            if (_value == null)
                throw new ArgumentNullException("_value");

            return this.AddOrGetExisting(_key, _value, (DateTimeOffset?)null, _regionName);
        }
        public virtual ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue> AddOrGetExisting(TSpatialKey _key, ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue> _value, CacheItemPolicy _policy, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            if (_value == null)
                throw new ArgumentNullException("_value");

            if (_policy == null)
                throw new ArgumentNullException("_policy");

            return this.AddOrGetExisting(_key, _value, _policy.AbsoluteExpiration, _regionName);
        }
        public virtual ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue> AddOrGetExisting(TSpatialKey _key, ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue> _value, DateTimeOffset? _absoluteExpiration, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            if (_value == null)
                throw new ArgumentNullException("_value");

            if (_regionName != null)
                this.AddRegion(_regionName);

            _regionName = _regionName ?? BaseCache.DEFAULT_REGION;

            var _envelope = new Envelope(_key);
            var _expirationInMilliSeconds = _absoluteExpiration == null ? int.MaxValue : (_absoluteExpiration - DateTimeOffset.UtcNow).Value.TotalMilliseconds;
            var _objectCache = this._objectCaches[_regionName];
            var _spatialCache = this._spatialCaches[_regionName];
            var _syncLock = this._syncLocks[_regionName];

            using (var _readerLock = new ReaderLock(_syncLock, Timeout.Infinite, true))
            {
                var _existing = this.Get(_spatialCache, _key);
                if (_existing == null)
                {
                    using (new WriterLock(_readerLock))
                    {
                        _objectCache.Add(_value);
                        _spatialCache.Add(_envelope, _value, _expirationInMilliSeconds, () => this.Remove(_key, _regionName));

                        return _value;
                    }
                }

                return _existing;
            }
        }
        public virtual ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue> AddOrUpdateExisting(TObjectKeyValue _objectKeyValue, ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue> _value, DateTimeOffset? _absoluteExpiration, string _regionName = null)
        {
            if (_objectKeyValue == null)
                throw new ArgumentNullException("_objectKeyValue");

            if (_value == null)
                throw new ArgumentNullException("_value");

            if (_regionName != null)
                this.AddRegion(_regionName);

            _regionName = _regionName ?? BaseCache.DEFAULT_REGION;

            var _objectCache = this._objectCaches[_regionName];
            var _spatialCache = this._spatialCaches[_regionName];
            var _syncLock = this._syncLocks[_regionName];
            var _expirationInMilliSeconds = _absoluteExpiration == null ? int.MaxValue : (_absoluteExpiration - DateTimeOffset.UtcNow).Value.TotalMilliseconds;

            using (var _readerLock = new ReaderLock(_syncLock, Timeout.Infinite, true))
            {
                var _cacheItem = _objectCache.FirstOrDefault(_x => _x.ObjectKeyValue == _objectKeyValue);
                if (_cacheItem != null)
                {
                    var _envelope = new Envelope(_cacheItem.SpatialKey);
              
                    using (new WriterLock(_readerLock))
                    {
                        _objectCache.Remove(_cacheItem);
                        _spatialCache.Remove(_envelope, _cacheItem);
                    }
                }
              
                using (new WriterLock(_readerLock))
                {
                    var _newEnvelope = new Envelope(_value.SpatialKey);

                    _objectCache.Add(_value);
                    _spatialCache.Add(_newEnvelope, _value, _expirationInMilliSeconds, () => this.Remove(_value.SpatialKey, _regionName));
                }

                return _value;
            }
        }
        public virtual ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue> Update(TObjectKeyValue _objectKeyValue, ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue> _value, string _regionName = null)
        {
            if (_objectKeyValue == null)
                throw new ArgumentNullException("_objectKeyValue");

            if (_value == null)
                throw new ArgumentNullException("_value");

            return this.Update(_objectKeyValue, _value, (DateTimeOffset?)null, _regionName);
        }
        public virtual ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue> Update(TObjectKeyValue _objectKeyValue, ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue> _value, CacheItemPolicy _policy, string _regionName = null)
        {
            if (_objectKeyValue == null)
                throw new ArgumentNullException("_objectKeyValue");

            if (_value == null)
                throw new ArgumentNullException("_value");

            if (_policy == null)
                throw new ArgumentNullException("_policy");

            return this.Update(_objectKeyValue, _value, _policy.AbsoluteExpiration, _regionName);
        }
        public virtual ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue> Update(TObjectKeyValue _objectKeyValue, ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue> _value, DateTimeOffset? _absoluteExpiration, string _regionName = null)
        {
            if (_objectKeyValue == null)
                throw new ArgumentNullException("_objectKeyValue");

            if (_value == null)
                throw new ArgumentNullException("_value");

            _regionName = _regionName ?? BaseCache.DEFAULT_REGION;

            var _objectCache = this._objectCaches[_regionName];
            var _spatialCache = this._spatialCaches[_regionName];
            var _syncLock = this._syncLocks[_regionName];
            var _expirationInMilliSeconds = _absoluteExpiration == null ? int.MaxValue : (_absoluteExpiration - DateTimeOffset.UtcNow).Value.TotalMilliseconds;

            using (var _readerLock = new ReaderLock(_syncLock, Timeout.Infinite, true))
            {
                var _cacheItem = _objectCache.FirstOrDefault(_x => _x.ObjectKeyValue == _objectKeyValue);
                if (_cacheItem != null)
                {
                    var _envelope = new Envelope(_cacheItem.SpatialKey);
                    var _newEnvelope = new Envelope(_value.SpatialKey);

                    using (new WriterLock(_readerLock))
                    {
                        _objectCache.Remove(_cacheItem);
                        _spatialCache.Remove(_envelope, _cacheItem);
              
                        _objectCache.Add(_value);
                        _spatialCache.Add(_newEnvelope, _value, _expirationInMilliSeconds, () => this.Remove(_value.SpatialKey, _regionName));
                    }

                    return _value;
                }

                return null;
            }
        }
        public virtual void Remove(TSpatialKey _key, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            _regionName = _regionName ?? BaseCache.DEFAULT_REGION;

            var _objectCache = this._objectCaches[_regionName];
            var _spatialCache = this._spatialCaches[_regionName];
            var _syncLock = this._syncLocks[_regionName];
            var _envelope = new Envelope(_key);

            using (var _readerLock = new ReaderLock(_syncLock, Timeout.Infinite, true))
            {
                var _existing = this.Get(_spatialCache, _key);
                if (_existing != null)
                {
                    using (new WriterLock(_readerLock))
                    {
                        _objectCache.Remove(_existing);
                        _spatialCache.Remove(_envelope, _existing);
                    }
                }
            }
        }
        public virtual IEnumerable<ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue>> Query(TSpatialKey _key, double _distanceInMeters, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            _regionName = _regionName ?? BaseCache.DEFAULT_REGION;

            var _spatialCache = this._spatialCaches[_regionName];
            var _syncLock = this._syncLocks[_regionName];

            var _envelope = new Envelope(_key);
            _envelope.ExpandBy(_distanceInMeters * 0.0000089928);

            using (new ReaderLock(_syncLock))
            {
                return _spatialCache.Query(_envelope)
                    .Where(_x => MathExtension.HaversineInMeters(_x.SpatialKey, _key) <= _distanceInMeters)
                    .OrderBy(_x => MathExtension.HaversineInMeters(_x.SpatialKey, _key));
            }
        }
        public virtual IEnumerable<ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue>> Intersect(IPolygon _polygon, string _regionName = null)
        {
            if (_polygon == null)
                throw new ArgumentNullException("_polygon");

            _regionName = _regionName ?? BaseCache.DEFAULT_REGION;

            var _spatialCache = this._spatialCaches[_regionName];
            var _syncLock = this._syncLocks[_regionName];

            var _envelope = _polygon.EnvelopeInternal;

            using (new ReaderLock(_syncLock))
            {
                return _spatialCache.Query(_envelope)
                    .Where(_x => _polygon.Intersects(new Point(_x.SpatialKey.X, _x.SpatialKey.Y)));
            }
        }
        public virtual IDictionary<TSpatialKey, ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue>> GetValues(IEnumerable<TSpatialKey> _keys, string _regionName = null)
        {
            if (_keys == null)
                throw new ArgumentNullException("_keys");

            return _keys.ToDictionary(_x => _x, _x => this.Get(_x, _regionName));
        }
        public virtual CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(IEnumerable<TSpatialKey> _keys, string _regionName = null)
        {
            if (_keys == null)
                throw new ArgumentNullException("_keys");

            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        public virtual IEnumerator<KeyValuePair<TSpatialKey, ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue>>> GetEnumerator()
        {
            return new Dictionary<TSpatialKey, ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue>>.Enumerator();
        }

        protected override void AddRegion(string _regionName)
        {
            if (string.IsNullOrEmpty(_regionName))
                throw new ArgumentNullException("_regionName");

            var _quadtree = this._spatialCaches.Where(_x => _x.Key == _regionName).Select(_x => _x.Value).FirstOrDefault();
            if (_quadtree == null)
            {
                this._syncLocks.TryAdd(_regionName, new ReaderWriterLockSlim());
                this._objectCaches.TryAdd(_regionName, new List<ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue>>());
                this._spatialCaches.TryAdd(_regionName, new Quadtree<ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue>>());
            }
        }

        private ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue> Get(Quadtree<ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectKeyValue>> _spatialCache, TSpatialKey _key)
        {
            var _envelope = new Envelope(_key);
            return _spatialCache.Query(_envelope).FirstOrDefault(_x => System.Math.Abs(_x.SpatialKey.Distance(_key) - 0) < double.Epsilon);
        }
    }
}