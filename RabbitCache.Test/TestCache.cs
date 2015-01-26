using System;
using System.Runtime.Caching;
using RabbitCache.Caches.Interfaces;

namespace RabbitCache.Test
{
    public class TestCache : ICache
    {
        public TestCache()
        {
            
        }
        public TestCache(string _name, DefaultCacheCapabilities _defaultCacheCapabilities)
        {
            this.DefaultCacheCapabilities = _defaultCacheCapabilities;
            this.Name = _name;
        }

        public string Name { get; private set; }
        public DefaultCacheCapabilities DefaultCacheCapabilities { get; private set; }

        public object AddOrGetExisting(object _key, object _value, DateTimeOffset? _absoluteExpiration = null,
            string _regionName = null)
        {
            return new object();
        }

        public object AddOrUpdateExisting(object _objectKeyValue, object _value, DateTimeOffset? _absoluteExpiration = null,
            string _regionName = null)
        {
            return new object();
        }
    }
}