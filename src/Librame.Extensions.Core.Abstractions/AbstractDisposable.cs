#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象可处置对象。
    /// </summary>
    public abstract class AbstractDisposable : IDisposable
    {
        private bool _disposed = false;


        /// <summary>
        /// 如果已处置则抛出异常。
        /// </summary>
        protected virtual void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }


        /// <summary>
        /// 释放对象。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放对象。
        /// </summary>
        /// <param name="disposing">是否立即释放。</param>
        protected virtual void Dispose(bool disposing)
        {
            // 如果不释放或已释放
            if (!disposing || _disposed)
                return;

            DisposeCore();

            if (disposing)
                _disposed = disposing;
        }

        /// <summary>
        /// 释放核心对象。
        /// </summary>
        protected abstract void DisposeCore();
    }
}
