using System;

namespace RabbitCache.Contracts.Interfaces
{
    public interface ICacheEntry
    {
        Type CacheType { get; set; }
        object Key { get; set; }
        object Value { get; set; }
        DateTimeOffset? ExpireAt { get; set; }
        string RegionName { get; set; }
    }
}