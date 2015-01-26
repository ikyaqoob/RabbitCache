using System;
using System.Collections.Generic;

namespace RabbitCache.Extensions
{
    public static class DictionaryExtension
    {
        public static void Add<TKey, TValue>(this IDictionary<TKey, TValue> _dictionary, TKey _key, TValue _value, double _timeoutInMilliSeconds, Action _callback)
        {
            _dictionary.Add(new KeyValuePair<TKey, TValue>(_key, _value), _timeoutInMilliSeconds, _callback);
        }
        public static void Add<TKey, TValue>(this IDictionary<TKey, TValue> _dictionary, KeyValuePair<TKey, TValue> _item, double _timeoutInMilliSeconds, Action _callback)
        {
            _dictionary.Add(_item);
            _dictionary.SetTimeout(_item, _timeoutInMilliSeconds, _callback);
        }
    }
}