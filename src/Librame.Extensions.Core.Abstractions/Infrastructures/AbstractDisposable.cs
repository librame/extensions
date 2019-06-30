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
    /// <typeparam name="TDisposable">指定的可处置对象类型。</typeparam>
    public abstract class AbstractDisposable<TDisposable> : AbstractDisposable, IDisposable<TDisposable>
    {
        /// <summary>
        /// 获取可处置对象类型。
        /// </summary>
        /// <returns>返回 <see cref="Type"/>。</returns>
        protected override Type GetDisposableType()
        {
            return typeof(TDisposable);
        }
    }

    /// <summary>
    /// 抽象可处置对象。
    /// </summary>
    public abstract class AbstractDisposable : IDisposable
    {
        private bool _disposed;

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
        /// 处置对象。
        /// </summary>
        public virtual void Dispose()
        {
            _disposed = true;
        }
    }
}
