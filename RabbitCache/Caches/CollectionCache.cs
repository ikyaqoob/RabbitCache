using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using RabbitCache.Caches.Interfaces;
using RabbitCache.Caches.Locking;
using RabbitCache.Extensions;

namespace RabbitCache.Caches
{
    public class CollectionCache<TKey, TValue> : BaseCache, ICollectionCache<TKey, TValue>
        where TKey : class 
        where TValue : class
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<TKey, IList<TValue>>>  _collectionCaches;

        public override DefaultCacheCapabilities DefaultCacheCapabilities
        {
            get { return DefaultCacheCapabilities.CacheRegions | DefaultCacheCapabilities.InMemoryProvider | DefaultCacheCapabilities.AbsoluteExpirations; }
        }

        public CollectionCache()
            : this(Guid.NewGuid().ToString("N"))
        {
        }
        public CollectionCache(string _name)
            : base(_name)
        {
            this._collectionCaches = new ConcurrentDictionary<string, ConcurrentDictionary<TKey, IList<TValue>>>();

            this.AddRegion(BaseCache.DEFAULT_REGION);
        }


        // TODO Create ICollectionCacheItem / CollectionCacheItem
        public virtual long GetCount(TKey _key, string _regionName = null)
        {
            if (_key == null) 
                throw new ArgumentNullException("_key");

            return this.Get(_key, _regionName).Count();
        }
        public virtual bool Contains(TKey _key, TValue _value, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            if (_value == null) 
                throw new ArgumentNullException("_value");

            return this.Get(_key, _regionName).Contains(_value);
        }
        public virtual IEnumerable<TValue> Get(TKey _key, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            _regionName = _regionName ?? BaseCache.DEFAULT_REGION;

            var _collectionCache = this._collectionCaches[_regionName];
            var _syncLock = this._syncLocks[_regionName];

            using (new ReaderLock(_syncLock))
            {
                try
                {
                    return _collectionCache[_key];
                }
                catch (KeyNotFoundException)
                {
                    return new List<TValue>();
                }
            }
        }
        public virtual object AddOrGetExisting(object _key, object _value, DateTimeOffset? _absoluteExpiration = null, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            if (_value == null)
                throw new ArgumentNullException("_value");

            var _realKey = (TKey)_key;
            var _realObject = (TValue)_value;

            return this.AddOrGetExisting(_realKey, _realObject, _absoluteExpiration, _regionName);
        }
        public virtual object AddOrUpdateExisting(object _objectKeyValue, object _value, DateTimeOffset? _absoluteExpiration = null, string _regionName = null)
        {
            if (_objectKeyValue == null)
                throw new ArgumentNullException("_objectKeyValue");

            if (_value == null)
                throw new ArgumentNullException("_value");

            var _realKey = (TKey)_objectKeyValue;
            var _realValue = (TValue)_value;

            return this.AddOrUpdateExisting(_realKey, _realValue, _absoluteExpiration, _regionName);
        }
        public virtual TValue AddOrGetExisting(TKey _key, TValue _value, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            if (_value == null)
                throw new ArgumentNullException("_value");

            return this.AddOrGetExisting(_key, _value, (DateTimeOffset?)null, _regionName);
        }
        public virtual TValue AddOrGetExisting(TKey _key, TValue _value, CacheItemPolicy _policy, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            if (_value == null)
                throw new ArgumentNullException("_value");

            if (_policy == null)
                throw new ArgumentNullException("_policy");

            return this.AddOrGetExisting(_key, _value, _policy.AbsoluteExpiration, _regionName);
        }
        public virtual TValue AddOrGetExisting(TKey _key, TValue _value, DateTimeOffset? _absoluteExpiration, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            if (_value == null)
                throw new ArgumentNullException("_value");

            if (_regionName != null)
                this.AddRegion(_regionName);

            _regionName = _regionName ?? BaseCache.DEFAULT_REGION;

            var _expirationInMilliSeconds = _absoluteExpiration == null ? int.MaxValue : (_absoluteExpiration - DateTimeOffset.UtcNow).Value.TotalMilliseconds;
            var _collectionCache = this._collectionCaches[_regionName];
            var _syncLock = this._syncLocks[_regionName];

            using (var _readerLock = new ReaderLock(_syncLock, Timeout.Infinite, true))
            {
                IList<TValue> _existing;
                _collectionCache.TryGetValue(_key, out _existing);

                using (new WriterLock(_readerLock))
                {
                    if (_existing == null)
                        _collectionCache.TryAdd(_key, new List<TValue>());
                    
                    var _list = _collectionCache[_key];
                    _list.Add(_value, _expirationInMilliSeconds, () => this.Remove(_key, _value, _regionName));
                    
                    return _value;
                }
            }
        }
        public virtual TValue AddOrUpdateExisting(TKey _key, TValue _value, DateTimeOffset? _absoluteExpiration, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            if (_value == null)
                throw new ArgumentNullException("_value");

            if (_regionName != null)
                this.AddRegion(_regionName);

            _regionName = _regionName ?? BaseCache.DEFAULT_REGION;

            var _collectionCache = this._collectionCaches[_regionName];
            var _syncLock = this._syncLocks[_regionName];
            var _expirationInMilliSeconds = _absoluteExpiration == null ? int.MaxValue : (_absoluteExpiration - DateTimeOffset.UtcNow).Value.TotalMilliseconds;

            using (var _readerLock = new ReaderLock(_syncLock, Timeout.Infinite, true))
            {
                IList<TValue> _existing;
                _collectionCache.TryGetValue(_key, out _existing);

                using (new WriterLock(_readerLock))
                {
                    if (_existing == null)
                        _collectionCache.TryAdd(_key, new List<TValue>());

                    var _list = _collectionCache[_key];
                    if (_list.Contains(_value))
                        _list.Remove(_value);

                    _list.Add(_value, _expirationInMilliSeconds, () => this.Remove(_key, _value, _regionName));

                    return _value;
                }
            }
        }
        public virtual TValue Update(TKey _key, TValue _value, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            if (_value == null)
                throw new ArgumentNullException("_value");

            return this.Update(_key, _value, (DateTimeOffset?)null, _regionName);
        }
        public virtual TValue Update(TKey _key, TValue _value, CacheItemPolicy _policy, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            if (_value == null)
                throw new ArgumentNullException("_value");

            if (_policy == null)
                throw new ArgumentNullException("_policy");

            return this.Update(_key, _value, _policy.AbsoluteExpiration, _regionName);
        }
        public virtual TValue Update(TKey _key, TValue _value, DateTimeOffset? _absoluteExpiration, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            if (_value == null)
                throw new ArgumentNullException("_value");

            _regionName = _regionName ?? BaseCache.DEFAULT_REGION;

            var _collectionCache = this._collectionCaches[_regionName];
            var _syncLock = this._syncLocks[_regionName];
            var _expirationInMilliSeconds = _absoluteExpiration == null ? int.MaxValue : (_absoluteExpiration - DateTimeOffset.UtcNow).Value.TotalMilliseconds;

            using (var _readerLock = new ReaderLock(_syncLock, Timeout.Infinite, true))
            {
                IList<TValue> _existing;
                _collectionCache.TryGetValue(_key, out _existing);

                if (_existing != null)
                {
                    using (new WriterLock(_readerLock))
                    {
                        var _list = _collectionCache[_key];

                        _list.Remove(_value);
                        _list.Add(_value, _expirationInMilliSeconds, () => this.Remove(_key, _value, _regionName));
                    }

                    return _value;
                }

                return null;
            }
        }
        public virtual void Remove(TKey _key, TValue _value, string _regionName = null)
        {
            if (_key == null)
                throw new ArgumentNullException("_key");

            _regionName = _regionName ?? BaseCache.DEFAULT_REGION;

            var _collectionCache = this._collectionCaches[_regionName];
            var _syncLock = this._syncLocks[_regionName];

            using (var _readerLock = new ReaderLock(_syncLock, Timeout.Infinite, true))
            {
                IList<TValue> _existing;
                _collectionCache.TryGetValue(_key, out _existing);

                if (_existing != null)
                {
                    using (new WriterLock(_readerLock))
                    {
                        var _list = _collectionCache[_key];
                        _list.Remove(_value);
                    }
                }
            }
        }

        public virtual IDictionary<TKey, IEnumerable<TValue>> GetValues(IEnumerable<TKey> _keys, string _regionName = null)
        {
            if (_keys == null)
                throw new ArgumentNullException("_keys");

            return _keys.ToDictionary(_x => _x, _x => this.Get(_x, _regionName));
        }
        public virtual CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(IEnumerable<TKey> _keys, string _regionName = null)
        {
            if (_keys == null)
                throw new ArgumentNullException("_keys");

            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        public IEnumerator<KeyValuePair<TKey, IList<TValue>>> GetEnumerator()
        {
            return new Dictionary<TKey, IList<TValue>>.Enumerator();
        }

        protected override void AddRegion(string _regionName)
        {
            if (string.IsNullOrEmpty(_regionName))
                throw new ArgumentNullException("_regionName");

            var _containsKey = this._collectionCaches.ContainsKey(_regionName);
            if (!_containsKey)
            {
                this._syncLocks.TryAdd(_regionName, new ReaderWriterLockSlim());
                this._collectionCaches.TryAdd(_regionName, new ConcurrentDictionary<TKey, IList<TValue>>());
            }
        }
    }
}