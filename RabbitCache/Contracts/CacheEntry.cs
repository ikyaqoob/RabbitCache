using System;

namespace RabbitCache.Contracts
{
    public class CacheEntry
    {
        public virtual Type CacheType { get; set; }
        public virtual object Key { get; set; }
        public virtual object Value { get; set; }
        public virtual DateTimeOffset? ExpireAt { get; set; }
        public virtual string RegionName { get; set; }
    }
}