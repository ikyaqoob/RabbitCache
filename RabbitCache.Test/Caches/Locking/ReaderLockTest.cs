using System.Threading;
using NUnit.Framework;
using RabbitCache.Caches.Locking;

namespace RabbitCache.Test.Caches.Locking
{
    [TestFixture]
    public class ReaderLockTest
    {
        [Test]
        public void ConstructorInitializesLockTest()
        {
            var _lock = new ReaderWriterLockSlim();
            using (var _readerLock = new ReaderLock(_lock))
            {
                Assert.AreEqual(_lock, _readerLock.Lock);
            }

            Assert.IsNotNull(_lock);
        }
        [Test]
        public void ConstructorInitializesTimeoutTest()
        {
            const int TIMEOUT = 100;

            var _lock = new ReaderWriterLockSlim();
            using (var _readerLock = new ReaderLock(_lock, TIMEOUT))
            {
                Assert.AreEqual(TIMEOUT, _readerLock.Timeout);
            }

            Assert.IsNotNull(_lock);
        }
        [Test]
        public void ConstructorInitializesIsUpgradableTest()
        {
            const bool IS_UPGRADABLE = true;

            var _lock = new ReaderWriterLockSlim();
            using (var _readerLock = new ReaderLock(_lock, -1, IS_UPGRADABLE))
            {
                Assert.AreEqual(IS_UPGRADABLE, _readerLock.IsUpgradable);
            }
        }

        [Test]
        public void ConstructorAquiresLockTest()
        {
            var _lock = new ReaderWriterLockSlim();

            using (var _readerLock = new ReaderLock(_lock))
            {
                Assert.AreEqual(_lock, _readerLock.Lock);
                Assert.IsTrue(_readerLock.Lock.IsReadLockHeld);
            }

            Assert.IsNotNull(_lock);
            Assert.IsFalse(_lock.IsReadLockHeld);
        }
        [Test]
        public void ConstructorAquiresLockWhenIsUpgradableTest()
        {
            var _lock = new ReaderWriterLockSlim();

            using (var _readerLock = new ReaderLock(_lock, -1, true))
            {
                Assert.AreEqual(_lock, _readerLock.Lock);
                Assert.IsTrue(_readerLock.Lock.IsUpgradeableReadLockHeld);
            }

            Assert.IsNotNull(_lock);
            Assert.IsFalse(_lock.IsUpgradeableReadLockHeld);
        }

        [Test]
        public void DisposeReleasesLockTest()
        {
            var _lock = new ReaderWriterLockSlim();

            using (var _readerLock = new ReaderLock(_lock))
            {
                Assert.AreEqual(_lock, _readerLock.Lock);
            }

            Assert.IsNotNull(_lock);
            Assert.IsFalse(_lock.IsReadLockHeld);
        }
        [Test]
        public void DisposeReleasesLockWhenIsUpgradableTest()
        {
            var _lock = new ReaderWriterLockSlim();

            using (var _readerLock = new ReaderLock(_lock))
            {
                Assert.AreEqual(_lock, _readerLock.Lock);
            }

            Assert.IsNotNull(_lock);
            Assert.IsFalse(_lock.IsUpgradeableReadLockHeld);
        }
    }
}