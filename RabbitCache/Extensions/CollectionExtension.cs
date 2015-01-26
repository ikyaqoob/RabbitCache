using System;
using System.Collections.Generic;
using System.Timers;

namespace RabbitCache.Extensions
{
    public static class CollectionExtension
    {
        public static void Add<T>(this ICollection<T> _collection, T _item, double _timeoutInMilliSeconds, Action _callback)
        {
            _collection.Add(_item);
            _collection.SetTimeout(_item, _timeoutInMilliSeconds, _callback);
        }

        internal static void SetTimeout<T>(this ICollection<T> _collection, T _item, double _timeoutInMilliSeconds, Action _callback)
        {
            var _timer = new Timer(_timeoutInMilliSeconds);
            _timer.Elapsed += (_sender, _e) => 
                {
                    _callback();
                    _timer.Stop();
                    _timer.Dispose();
                };

            _timer.Start();
        }
    }
}