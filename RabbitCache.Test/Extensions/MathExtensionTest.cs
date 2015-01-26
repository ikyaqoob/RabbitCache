using GeoAPI.Geometries;
using NUnit.Framework;
using RabbitCache.Extensions;

namespace RabbitCache.Test.Extensions
{
    [TestFixture]
    public class MathExtensionTest
    {
        [Test]
        public void HaversineInMetersTest()
        {
            var _haversine = MathExtension.HaversineInMeters(new Coordinate(40.708210, -74.006074), new Coordinate(40.055454, -74.409822));
            Assert.AreEqual(49047, _haversine);
        }
        [Test]
        public void HaversineInMilesTest()
        {
            var _haversine = MathExtension.HaversineInMiles(new Coordinate(40.708210, -74.006074), new Coordinate(40.055454, -74.409822));
            Assert.AreEqual(30486, _haversine);
        }
    }
}