using System.Threading;
using NUnit.Framework;
using RabbitCache.Caches.Locking;

namespace RabbitCache.Test.Caches.Locking
{
    [TestFixture]
    public class WriterLockTest
    {
        [Test]
        public void ConstructorInitializesLockTest()
        {
            var _lock = new ReaderWriterLockSlim();
            
            using (var _writerLock = new WriterLock(_lock))
            {
                Assert.AreEqual(_lock, _writerLock.Lock);
            }

            Assert.IsNotNull(_lock);
        }
        [Test]
        public void ConstructorInitializesTimeoutTest()
        {
            const int TIMEOUT = 100;

            using (var _writerLock = new WriterLock(new ReaderWriterLockSlim(), TIMEOUT))
            {
                Assert.AreEqual(TIMEOUT, _writerLock.Timeout);
            }
        }
        [Test]
        public void ConstructorInitializesReaderLockTest()
        {
            const int TIMEOUT = 100;
            var _lock = new ReaderWriterLockSlim();
            var _readerLock = new ReaderLock(_lock, TIMEOUT, true);

            using (var _writerLock = new WriterLock(_readerLock))
            {
                Assert.AreEqual(_lock, _writerLock.Lock);
                Assert.AreEqual(TIMEOUT, _writerLock.Timeout);
            }

            Assert.IsNotNull(_lock);
        }
        [Test]
        public void ConstructorThrowsLockRecursionExceptionWhenLockIsNotUpdatableTest()
        {
            var _lock = new ReaderWriterLockSlim();
            var _readerLock = new ReaderLock(_lock);

            Assert.Throws<System.Threading.LockRecursionException>(() => new WriterLock(_readerLock));
            Assert.IsNotNull(_lock);
        }

        [Test]
        public void ConstructorAquiresLockTest()
        {
            var _lock = new ReaderWriterLockSlim();

            using (var _writerLock = new WriterLock(_lock))
            {
                Assert.AreEqual(_lock, _writerLock.Lock);
                Assert.IsTrue(_writerLock.Lock.IsWriteLockHeld);
            }

            Assert.IsNotNull(_lock);
            Assert.IsFalse(_lock.IsWriteLockHeld);
        }
        [Test]
        public void DisposeReleasesLockTest()
        {
            var _lock = new ReaderWriterLockSlim();

            using (var _writerLock = new WriterLock(_lock))
            {
                Assert.AreEqual(_lock, _writerLock.Lock);
            }

            Assert.IsNotNull(_lock);
            Assert.IsFalse(_lock.IsWriteLockHeld);
        }
    }
}