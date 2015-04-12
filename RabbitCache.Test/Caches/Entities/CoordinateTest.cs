using NUnit.Framework;
using RabbitCache.Caches.Entities;

namespace RabbitCache.Test.Caches.Entities
{
    [TestFixture]
    public class CoordinateTest : TestBase
    {
        [SetUp]
        public override void SetupFixture()
        {
        }

        [Test]
        public void ConstructorInitializesPropertiesTest()
        {
            var _coordinate = new Coordinate();

            Assert.AreEqual(0.00, _coordinate.X);
            Assert.AreEqual(0.00, _coordinate.Y);
            Assert.AreEqual(0.00, _coordinate.Z);
        }

        [Test]
        public void LatitudeTest()
        {
            var _coordinate = new Coordinate { Latitude = 10.00 };

            Assert.AreEqual(10.00, _coordinate.Y);
            Assert.AreEqual(10.00, _coordinate.Latitude);
        }
        [Test]
        public void LongitudeTest()
        {
            var _coordinate = new Coordinate { Longitude = 20.00 };

            Assert.AreEqual(20.00, _coordinate.X);
            Assert.AreEqual(20.00, _coordinate.Longitude);
        }

        [Test]
        public void ToPointTest()
        {
            var _coordinate = new Coordinate { X = 10.00, Y = 20.00, Z = 30.00 };
            var _point = _coordinate.ToPoint();

            Assert.AreEqual(_coordinate.X, _point.X);
            Assert.AreEqual(_coordinate.Y, _point.Y);
            Assert.AreEqual(_coordinate.Z, _point.Z);
        }

        [Test]
        public void ToCoordinateTest()
        {
            var _coordinate = new Coordinate { X = 10.00, Y = 20.00, Z = 30.00 };
            var _coordinate2 = _coordinate.ToCoordinate();

            Assert.AreEqual(_coordinate.X, _coordinate2.X);
            Assert.AreEqual(_coordinate.Y, _coordinate2.Y);
            Assert.AreEqual(_coordinate.Z, _coordinate2.Z);
        }
     
    }
}