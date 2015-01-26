using System;
using System.Collections.Concurrent;
using System.Runtime.Caching;
using System.Threading;

namespace RabbitCache.Caches
{
    public abstract class BaseCache
    {
        protected const string DEFAULT_REGION = "DEFAULT_REGION";

        protected readonly string _name;
        protected readonly ConcurrentDictionary<string, ReaderWriterLockSlim> _syncLocks;

        public string Name { get { return this._name; } }
        public abstract DefaultCacheCapabilities DefaultCacheCapabilities { get; }

        protected BaseCache()
        {
            this._syncLocks = new ConcurrentDictionary<string, ReaderWriterLockSlim>();
        }
        protected BaseCache(string _name)
            : this()
        {
            if (string.IsNullOrEmpty(_name))
                throw new ArgumentNullException("_name");

            this._name = _name;
        }

        protected abstract void AddRegion(string _regionName);
    }
}
