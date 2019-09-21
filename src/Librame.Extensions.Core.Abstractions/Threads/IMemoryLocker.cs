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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 内存锁定器接口。
    /// </summary>
    public interface IMemoryLocker : IDisposable
    {
        /// <summary>
        /// 线程数。
        /// </summary>
        int ThreadsCount { get; }


        /// <summary>
        /// 释放。
        /// </summary>
        /// <param name="releaseCount">给定要释放的线程数（可选）。</param>
        /// <returns>返回整数。</returns>
        int Release(int? releaseCount = null);


        /// <summary>
        /// 等待。
        /// </summary>
        /// <param name="millisecondsTimeout">给定的毫秒超时数（可选）。</param>
        /// <param name="timeout">给定的超时（可选）。</param>
        /// <param name="cancellationToken">给定的取消令牌（可选）。</param>
        void Wait(int? millisecondsTimeout = null, TimeSpan? timeout = null,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// 异步等待。
        /// </summary>
        /// <param name="millisecondsTimeout">给定的毫秒超时数（可选）。</param>
        /// <param name="timeout">给定的超时（可选）。</param>
        /// <param name="cancellationToken">给定的取消令牌（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task WaitAsync(int? millisecondsTimeout = null, TimeSpan? timeout = null,
            CancellationToken? cancellationToken = null);
    }
}
