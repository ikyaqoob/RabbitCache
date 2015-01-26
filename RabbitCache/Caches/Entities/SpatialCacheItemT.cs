using GeoAPI.Geometries;
using RabbitCache.Caches.Entities.Interfaces;

namespace RabbitCache.Caches.Entities
{
    public class SpatialCacheItem<TSpatialKey, TSpatialValue, TObjectValue> : SpatialCacheItem, ISpatialCacheItem<TSpatialKey, TSpatialValue, TObjectValue>
        where TSpatialKey : Coordinate
        where TSpatialValue : class
        where TObjectValue : class
    {
        public virtual TSpatialKey SpatialKey { get; set; }
        public virtual TSpatialValue SpatialValue { get; set; }
        public virtual TObjectValue ObjectKeyValue { get; set; }

        public SpatialCacheItem()
        {
            
        }
        public SpatialCacheItem(TSpatialKey _spatialKey, TSpatialValue _spatialValue, TObjectValue _objectValue)
        {
            this.SpatialKey = _spatialKey;
            this.SpatialValue = _spatialValue;
            this.ObjectKeyValue = _objectValue;
        }
    }
}