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
        /// 获取可处置对象类型。
        /// </summary>
        /// <returns>返回 <see cref="Type"/>。</returns>
        protected virtual Type GetDisposableType()
            => GetType();

        /// <summary>
        /// 如果已处置则抛出异常。
        /// </summary>
        protected virtual void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetDisposableType()?.Name);
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
        }
        /// <summary>
        /// 释放资源。
        /// </summary>
        /// <param name="disposing">是否立即释放。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _disposed = true;
        }
    }
}
