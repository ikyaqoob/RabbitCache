using System;
using System.Linq;
using System.Timers;
using GeoAPI.Geometries;
using NetTopologySuite.Index.Quadtree;

namespace RabbitCache.Extensions
{
    public static class QuadtreeExtension
    {
        public static bool Contains<T>(this Quadtree<T> _quadtree, Envelope _envelope) where T : class
        {
            return _quadtree.Query(_envelope).FirstOrDefault() != null;
        }

        public static void Add<T>(this Quadtree<T> _quadtree, Envelope _envelope, T _item) where T : class
        {
            _quadtree.Insert(_envelope, _item);
        }
        public static void Add<T>(this Quadtree<T> _quadtree, Envelope _envelope, T _item, double _timeoutInMilliSeconds, Action _callback) where T : class
        {
            _quadtree.Insert(_envelope, _item);
            _quadtree.SetTimeout(_envelope, _item, _timeoutInMilliSeconds, _callback);
        }

        internal static void SetTimeout<T>(this Quadtree<T> _quadtree, Envelope _envelope, T _item, double _timeoutInMilliSeconds, Action _callback)
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