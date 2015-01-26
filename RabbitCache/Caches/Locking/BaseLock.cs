using System;
using System.Threading;

namespace RabbitCache.Caches.Locking
{
    public abstract class BaseLock : IDisposable
    {
        private bool _disposed;
        private readonly int _timeout;
        private readonly bool _isUpgradable;
        private readonly ReaderWriterLockSlim _lock;

        protected internal int Timeout
        {
            get
            {
                return this._timeout;
            }
        }
        protected internal bool IsUpgradable
        {
            get { return this._isUpgradable; }
        }
        protected internal ReaderWriterLockSlim Lock
        {
            get
            {
                if (this._disposed)
                    throw new ObjectDisposedException("Can't access lock for disposed object.");

                return this._lock;
            }
        }

        protected BaseLock(ReaderWriterLockSlim _lock, int _timeout = System.Threading.Timeout.Infinite, bool _isUpgradable = false)
            : base()
        {
            if (_lock == null)
                throw new ArgumentNullException("_lock");

            this._lock = _lock;
            this._timeout = _timeout;
            this._isUpgradable = _isUpgradable;
            
            this.Aquire();
        }

        ~BaseLock()
        {
            this.Dispose(false);
        }

        protected abstract bool IsLocked();
        protected abstract void Aquire();
        protected abstract void Release();

        // IDisposable.
        public void Dispose()
        {
            this.Dispose(true);
        }
        
        protected virtual void Dispose(bool _disposing)
        {
            if (!this._disposed)
            {
                if (_disposing)
                {
                    this.Release();
                    GC.SuppressFinalize(this);
                }

                this._disposed = true;
            }
        }
    }
}