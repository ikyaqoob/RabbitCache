using System;
using System.Threading;

namespace RabbitCache.Caches.Locking
{
    public sealed class WriterLock : BaseLock
    {
        public WriterLock(ReaderWriterLockSlim _lock, int _timeout = System.Threading.Timeout.Infinite)
            : base(_lock, _timeout)
        {
        }
        
        public WriterLock(ReaderLock _readerLock)
            : base(_readerLock.Lock, _readerLock.Timeout)
        {
            if (!this.Lock.IsUpgradeableReadLockHeld)
                throw new InvalidOperationException("!this.Lock.IsUpgradeableReadLockHeld");
        }

        protected override bool IsLocked()
        {
            return this.Lock.IsWriteLockHeld;
        }
        protected override void Aquire()
        {
            var _success = this.Lock.TryEnterWriteLock(this.Timeout);
            if (!_success)
                throw new InvalidOperationException("!_success");
        }
        protected override void Release()
        {
            if (!this.IsLocked())
                return;

            this.Lock.ExitWriteLock();
        }
    }
}