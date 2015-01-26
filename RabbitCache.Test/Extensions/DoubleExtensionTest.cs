using NUnit.Framework;
using RabbitCache.Extensions;

namespace RabbitCache.Test.Extensions
{
    [TestFixture]
    public class DoubleExtensionTest
    {
        [Test]
        public void ToRadianTest()
        {
            const double DOUBLE = 21.2376;
            Assert.AreEqual((System.Math.PI / 180 * DOUBLE), DOUBLE.ToRadian());
        }
    }
}