using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitCache.Caches.Entities;
using RabbitCache.Contracts;

namespace RabbitCache.Caches.Serialization
{
    /// <summary>
    /// Resolver that only serializes simple types and object types located directly on the CacheEntry object. 
    /// Type, string, DateTimeOffset, TimeSpan and Nullable.
    /// </summary>
    public class CacheEntryContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo _member, MemberSerialization _memberSerialization)
        {
            if (_member == null)
                throw new ArgumentNullException("_member");

            var _property = base.CreateProperty(_member, _memberSerialization);

            if (_property.PropertyType.IsValueType)
                return _property;

            if (_property.PropertyType == typeof(string))
                return _property;

            if (_property.PropertyType == typeof(Type))
                return _property;

            if (_property.PropertyType == typeof(TimeSpan))
                return _property;

            if (_property.PropertyType == typeof(DateTimeOffset))
                return _property;

            if (_property.PropertyType.IsGenericType && _property.PropertyType.GetGenericTypeDefinition() == typeof(Tuple<,>))
                return _property;

            if (_property.PropertyType.IsGenericType && _property.PropertyType.GetGenericTypeDefinition() == typeof(Tuple<,,>))
                return _property;

            if (_property.PropertyType.IsGenericType && _property.PropertyType.GetGenericTypeDefinition() == typeof(Tuple<,,,>))
                return _property;

            if (_property.PropertyType.IsGenericType && _property.PropertyType.GetGenericTypeDefinition() == typeof(Tuple<,,,,>))
                return _property;

            if (_property.PropertyType.IsGenericType && _property.PropertyType.GetGenericTypeDefinition() == typeof(Tuple<,,,,,>))
                return _property;

            if (_property.PropertyType.IsGenericType && _property.PropertyType.GetGenericTypeDefinition() == typeof(Tuple<,,,,,,>))
                return _property;

            if (_property.PropertyType.IsGenericType && _property.PropertyType.GetGenericTypeDefinition() == typeof(Tuple<,,,,,,,>))
                return _property;

            if (_property.PropertyType.IsGenericType && _property.PropertyType.GetGenericTypeDefinition() == typeof(Tuple<,,,,,,,>))
                return _property;
            
            if (_property.DeclaringType.IsGenericType && _property.DeclaringType.GetGenericTypeDefinition() == typeof(Tuple<,>))
                return _property;

            if (_property.DeclaringType.IsGenericType && _property.DeclaringType.GetGenericTypeDefinition() == typeof(Tuple<,,>))
                return _property;

            if (_property.DeclaringType.IsGenericType && _property.DeclaringType.GetGenericTypeDefinition() == typeof(Tuple<,,,>))
                return _property;

            if (_property.DeclaringType.IsGenericType && _property.DeclaringType.GetGenericTypeDefinition() == typeof(Tuple<,,,,>))
                return _property;
            
            if (_property.DeclaringType.IsGenericType && _property.DeclaringType.GetGenericTypeDefinition() == typeof(Tuple<,,,,,>))
                return _property;
            
            if (_property.DeclaringType.IsGenericType && _property.DeclaringType.GetGenericTypeDefinition() == typeof(Tuple<,,,,,,>))
                return _property;

            if (_property.DeclaringType.IsGenericType && _property.DeclaringType.GetGenericTypeDefinition() == typeof(Tuple<,,,,,,,>))
                return _property;

            if (_property.PropertyType.IsGenericType && _property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                return _property;

            if (_property.PropertyType.IsSubclassOf(typeof(SpatialCacheItem)))
                return _property;

            if (_property.DeclaringType.IsSubclassOf(typeof(SpatialCacheItem)))
                return _property;

            if (_property.DeclaringType == typeof(CacheEntry))
                return _property;

            _property.ShouldSerialize = _x => false;
            return _property;
        }
    }
}