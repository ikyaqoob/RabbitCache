using System;
using System.Threading;

namespace RabbitCache.Caches.Locking
{
    public sealed class ReaderLock : BaseLock
    {
        public ReaderLock(ReaderWriterLockSlim _lock, int _timeout = System.Threading.Timeout.Infinite, bool _isUpgradable = false)
            : base(_lock, _timeout, _isUpgradable)
        {
        }

        protected override bool IsLocked()
        {
            return this.IsUpgradable ? this.Lock.IsUpgradeableReadLockHeld : this.Lock.IsReadLockHeld;
        }
        protected override void Aquire()
        {
            var _success = this.IsUpgradable 
                ? this.Lock.TryEnterUpgradeableReadLock(this.Timeout) 
                : this.Lock.TryEnterReadLock(this.Timeout);

            if (!_success)
                throw new InvalidOperationException("!_success");
        }
        protected override void Release()
        {
            if (!this.IsLocked())
                return;

            if (this.IsUpgradable)
                this.Lock.ExitUpgradeableReadLock();
            else
                this.Lock.ExitReadLock();
        }
    }
}