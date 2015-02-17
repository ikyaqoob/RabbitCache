using System;
using RabbitCache.Caches.Entities.Interfaces;

namespace RabbitCache.Test
{
    public class TestSpatialObjectKey : ISpatialObjectKey
    {
        public string UniqueIdentifier { get; protected set; }

        public TestSpatialObjectKey()
        {
            this.UniqueIdentifier = Guid.NewGuid().ToString("N");
        }
    }
}