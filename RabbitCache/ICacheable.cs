using RabbitCache.Caches.Interfaces;

namespace RabbitCache
{
    /// <summary>
    /// Contract for classes to be included as caches.
    /// </summary>
    public interface ICacheable<T> where T : class, ICache
    {
        
    }
}