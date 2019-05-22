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
    /// 抽象处置对象。
    /// </summary>
    /// <typeparam name="TDisposed">指定的已处置类型。</typeparam>
    public abstract class AbstractDispose<TDisposed> : AbstractDispose
    {
        /// <summary>
        /// 获取已处置类型。
        /// </summary>
        /// <returns>返回 <see cref="Type"/>。</returns>
        protected override Type GetDisposedType()
        {
            return typeof(TDisposed);
        }
    }

    /// <summary>
    /// 抽象处置对象。
    /// </summary>
    public abstract class AbstractDispose : IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// 获取已处置类型。
        /// </summary>
        /// <returns>返回 <see cref="Type"/>。</returns>
        protected abstract Type GetDisposedType();

        /// <summary>
        /// 如果已处置则抛出异常。
        /// </summary>
        protected virtual void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetDisposedType()?.Name);
        }

        /// <summary>
        /// 处置。
        /// </summary>
        public virtual void Dispose()
        {
            _disposed = true;
        }
    }
}
