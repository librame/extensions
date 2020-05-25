#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象可释放对象。
    /// </summary>
    public abstract class AbstractDisposable : IDisposable
    {
        private bool _disposed = false;


        /// <summary>
        /// 构造一个 <see cref="AbstractDisposable"/>。
        /// </summary>
        /// <param name="throwIfDisposed">如果已释放是否抛出异常（可选；默认不抛出异常）。</param>
        /// <param name="messageAction">给定的消息动作（可选；如可绑定记录日志等操作）。</param>
        public AbstractDisposable(bool throwIfDisposed = false,
            Action<string> messageAction = null)
        {
            ThrowIfDisposed = throwIfDisposed;
            MessageAction = messageAction;
        }


        /// <summary>
        /// 析构可处置对象。
        /// </summary>
        ~AbstractDisposable()
        {
            Dispose(false);
        }


        /// <summary>
        /// 如果已释放是否抛出异常。
        /// </summary>
        public bool ThrowIfDisposed { get; }

        /// <summary>
        /// 消息动作。
        /// </summary>
        public Action<string> MessageAction { get; set; }


        /// <summary>
        /// 释放类型。
        /// </summary>
        protected Type DisposeType
            => GetType();


        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        /// <param name="disposing">是否立即释放。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _disposed && ThrowIfDisposed)
                throw new ObjectDisposedException(DisposeType.Name);

            if (!_disposed)
            {
                if (disposing)
                {
                    DisposeManaged();
                    MessageAction?.Invoke("Dispose managed finish.");
                }

                DisposeUnmanaged();
                MessageAction?.Invoke("Dispose unmanaged finish.");
            }

            _disposed = true;
        }


        /// <summary>
        /// 释放托管资源。
        /// </summary>
        protected abstract void DisposeManaged();

        /// <summary>
        /// 释放非托管资源。
        /// </summary>
        protected virtual void DisposeUnmanaged()
        {
        }
    }
}
