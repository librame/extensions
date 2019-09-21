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
    /// 抽象内存锁定器静态扩展。
    /// </summary>
    public static class AbstractionMemoryLockerExtensions
    {
        /// <summary>
        /// 等待动作。
        /// </summary>
        /// <param name="locker">给定的 <see cref="IMemoryLocker"/>。</param>
        /// <param name="action">给定要等待的动作方法。</param>
        /// <param name="millisecondsTimeout">给定的毫秒超时数（可选）。</param>
        /// <param name="timeout">给定的超时（可选）。</param>
        /// <param name="cancellationToken">给定的取消令牌（可选）。</param>
        /// <param name="releaseCount">给定要释放的线程数（可选）。</param>
        public static void WaitAction(this IMemoryLocker locker, Action action,
            int? millisecondsTimeout = null, TimeSpan? timeout = null, CancellationToken? cancellationToken = null,
            int? releaseCount = null)
        {
            locker.NotNull(nameof(locker));
            action.NotNull(nameof(action));

            locker.Wait(millisecondsTimeout, timeout, cancellationToken);

            try
            {
                action.Invoke();
            }
            finally
            {
                locker.Release(releaseCount);
            }
        }

        /// <summary>
        /// 异步等待动作。
        /// </summary>
        /// <param name="locker">给定的 <see cref="IMemoryLocker"/>。</param>
        /// <param name="factory">给定要等待的动作方法。</param>
        /// <param name="millisecondsTimeout">给定的毫秒超时数（可选）。</param>
        /// <param name="timeout">给定的超时（可选）。</param>
        /// <param name="cancellationToken">给定的取消令牌（可选）。</param>
        /// <param name="releaseCount">给定要释放的线程数（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public static async Task WaitActionAsync(this IMemoryLocker locker, Func<Task> factory,
            int? millisecondsTimeout = null, TimeSpan? timeout = null, CancellationToken? cancellationToken = null,
            int? releaseCount = null)
        {
            locker.NotNull(nameof(locker));
            factory.NotNull(nameof(factory));

            await locker.WaitAsync(millisecondsTimeout, timeout, cancellationToken).ConfigureAwait(false);

            try
            {
                await factory.Invoke().ConfigureAwait(false);
            }
            finally
            {
                locker.Release(releaseCount);
            }
        }


        /// <summary>
        /// 等待工厂。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="locker">给定的 <see cref="IMemoryLocker"/>。</param>
        /// <param name="factory">给定要等待的工厂方法。</param>
        /// <param name="millisecondsTimeout">给定的毫秒超时数（可选）。</param>
        /// <param name="timeout">给定的超时（可选）。</param>
        /// <param name="cancellationToken">给定的取消令牌（可选）。</param>
        /// <param name="releaseCount">给定要释放的线程数（可选）。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        public static TResult WaitFactory<TResult>(this IMemoryLocker locker, Func<TResult> factory,
            int? millisecondsTimeout = null, TimeSpan? timeout = null, CancellationToken? cancellationToken = null,
            int? releaseCount = null)
        {
            locker.NotNull(nameof(locker));
            factory.NotNull(nameof(factory));

            locker.Wait(millisecondsTimeout, timeout, cancellationToken);

            try
            {
                return factory.Invoke();
            }
            finally
            {
                locker.Release(releaseCount);
            }
        }

        /// <summary>
        /// 等待工厂。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="locker">给定的 <see cref="IMemoryLocker"/>。</param>
        /// <param name="factory">给定要等待的工厂方法。</param>
        /// <param name="millisecondsTimeout">给定的毫秒超时数（可选）。</param>
        /// <param name="timeout">给定的超时（可选）。</param>
        /// <param name="cancellationToken">给定的取消令牌（可选）。</param>
        /// <param name="releaseCount">给定要释放的线程数（可选）。</param>
        /// <returns>返回 <see cref="Task{TResult}"/>。</returns>
        public static async Task<TResult> WaitFactoryAsync<TResult>(this IMemoryLocker locker, Func<Task<TResult>> factory,
            int? millisecondsTimeout = null, TimeSpan? timeout = null, CancellationToken? cancellationToken = null,
            int? releaseCount = null)
        {
            locker.NotNull(nameof(locker));
            factory.NotNull(nameof(factory));

            await locker.WaitAsync(millisecondsTimeout, timeout, cancellationToken).ConfigureAwait(false);

            try
            {
                return await factory.Invoke().ConfigureAwait(true);
            }
            finally
            {
                locker.Release(releaseCount);
            }
        }

    }
}
