using System;
using RabbitCache.Contracts.Interfaces;

namespace RabbitCache.Contracts
{
    public class CacheEntry : ICacheEntry
    {
        public virtual Type CacheType { get; set; }
        public virtual object Key { get; set; }
        public virtual object Value { get; set; }
        public virtual DateTimeOffset? ExpireAt { get; set; }
        public virtual string RegionName { get; set; }
    }
}