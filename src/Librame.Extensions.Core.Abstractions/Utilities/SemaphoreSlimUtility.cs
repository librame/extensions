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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Utilities
{
    /// <summary>
    /// <see cref="SemaphoreSlim"/> 实用工具。
    /// </summary>
    public static class SemaphoreSlimUtility
    {
        private static readonly Lazy<SemaphoreSlim> _semaphore
            = new Lazy<SemaphoreSlim>(() => new SemaphoreSlim(ExtensionSettings.ProcessorCount));


        /// <summary>
        /// 运行信号量。
        /// </summary>
        /// <param name="action">给定的动作。</param>
        public static void Run(Action action)
        {
            _semaphore.Value.Wait();

            action.NotNull(nameof(action)).Invoke();

            _semaphore.Value.Release();
        }

        /// <summary>
        /// 运行信号量，并返回结果。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="func">给定的工厂方法。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        public static TResult Run<TResult>(Func<TResult> func)
        {
            _semaphore.Value.Wait();

            var value = func.NotNull(nameof(func)).Invoke();

            _semaphore.Value.Release();

            return value;
        }


        /// <summary>
        /// 异步运行信号量。
        /// </summary>
        /// <param name="action">给定的动作。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        public static async Task RunTaskAsync(Action action, CancellationToken cancellationToken = default)
        {
            await _semaphore.Value.WaitAsync(cancellationToken).ConfigureAwait();

            await cancellationToken.RunOrCancelAsync(action).ConfigureAwait();

            _semaphore.Value.Release();
        }

        /// <summary>
        /// 异步运行信号量，并返回结果。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="func">给定的工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TResult}"/>。</returns>
        public static async Task<TResult> RunTaskAsync<TResult>(Func<TResult> func,
            CancellationToken cancellationToken = default)
        {
            await _semaphore.Value.WaitAsync(cancellationToken).ConfigureAwait();

            var value = await cancellationToken.RunOrCancelAsync(func).ConfigureAwait();

            _semaphore.Value.Release();

            return value;
        }


        /// <summary>
        /// 异步运行信号量。
        /// </summary>
        /// <param name="action">给定的动作。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask"/>。</returns>
        public static async ValueTask RunValueTaskAsync(Action action, CancellationToken cancellationToken = default)
        {
            await _semaphore.Value.WaitAsync(cancellationToken).ConfigureAwait();

            await cancellationToken.RunOrCancelValueAsync(action).ConfigureAwait();

            _semaphore.Value.Release();
        }

        /// <summary>
        /// 异步运行信号量，并返回结果。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="func">给定的工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TResult}"/>。</returns>
        public static async ValueTask<TResult> RunValueTaskAsync<TResult>(Func<TResult> func,
            CancellationToken cancellationToken = default)
        {
            await _semaphore.Value.WaitAsync(cancellationToken).ConfigureAwait();

            var value = await cancellationToken.RunOrCancelValueAsync(func).ConfigureAwait();

            _semaphore.Value.Release();

            return value;
        }

    }
}
