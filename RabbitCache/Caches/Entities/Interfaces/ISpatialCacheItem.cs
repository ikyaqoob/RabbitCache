namespace RabbitCache.Caches.Entities.Interfaces
{
    public interface ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectValue>
    {
        TSpatialKey SpatialKey { get; set; }
        TSpatialValue SpatialValue { get; set; }
        TObjectValue ObjectKeyValue { get; set; }
    }
}