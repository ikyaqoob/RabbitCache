using System;
using GeoAPI.Geometries;

namespace RabbitCache.Extensions
{
    public static class MathExtension
    {
        public static int HaversineInMiles(Coordinate _coordinate1, Coordinate _coordinate2)
        {
            if (_coordinate1 == null)
                throw new ArgumentNullException("_coordinate1");

            if (_coordinate2 == null)
                throw new ArgumentNullException("_coordinate2");

            const int RADIUS = 3960;

            var _decimalLatitude = (_coordinate1.Y - _coordinate2.Y).ToRadian();
            var _decimalLongitude = (_coordinate1.X - _coordinate2.X).ToRadian();
            var _distance = System.Math.Sin(_decimalLatitude / 2) * System.Math.Sin(_decimalLatitude / 2) + System.Math.Cos(_coordinate2.Y.ToRadian()) * System.Math.Cos(_coordinate1.Y.ToRadian()) * System.Math.Sin(_decimalLongitude / 2) * System.Math.Sin(_decimalLongitude / 2);

            return (int)(RADIUS * 2 * System.Math.Asin(System.Math.Min(1, System.Math.Sqrt(_distance))) * 1000);
        }
        public static int HaversineInMeters(Coordinate _coordinate1, Coordinate _coordinate2)
        {
            if (_coordinate1 == null)
                throw new ArgumentNullException("_coordinate1");

            if (_coordinate2 == null)
                throw new ArgumentNullException("_coordinate2");

            const int RADIUS = 6371;

            var _decimalLatitude = (_coordinate1.Y - _coordinate2.Y).ToRadian();
            var _decimalLongitude = (_coordinate1.X - _coordinate2.X).ToRadian();
            var _distance = System.Math.Sin(_decimalLatitude / 2) * System.Math.Sin(_decimalLatitude / 2) + System.Math.Cos(_coordinate2.Y.ToRadian()) * System.Math.Cos(_coordinate1.Y.ToRadian()) * System.Math.Sin(_decimalLongitude / 2) * System.Math.Sin(_decimalLongitude / 2);

            return (int)(RADIUS * 2 * System.Math.Asin(System.Math.Min(1, System.Math.Sqrt(_distance))) * 1000);
        }
    }
}