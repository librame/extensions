#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace System
{
    /// <summary>
    /// 抽象可释放。
    /// </summary>
    /// <typeparam name="TTarget">指定的目标类型。</typeparam>
    public abstract class AbstractDisposable<TTarget> : AbstractDisposable
    {
        /// <summary>
        /// 获取目标类型。
        /// </summary>
        /// <returns>返回 <see cref="Type"/>。</returns>
        protected override Type GetTargetType()
        {
            return typeof(TTarget);
        }
    }


    /// <summary>
    /// 抽象可释放。
    /// </summary>
    public abstract class AbstractDisposable : IDisposable
    {
        private bool _disposed;


        /// <summary>
        /// 获取目标类型。
        /// </summary>
        /// <returns>返回 <see cref="Type"/>。</returns>
        protected abstract Type GetTargetType();

        /// <summary>
        /// 如果已释放则抛出异常。
        /// </summary>
        protected virtual void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetTargetType()?.Name);
        }


        /// <summary>
        /// 释放。
        /// </summary>
        public virtual void Dispose()
        {
            _disposed = true;
        }
    }
}
