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

namespace Librame.Extensions
{
    /// <summary>
    /// 取消令牌静态扩展。
    /// </summary>
    public static class CancellationTokenExtensions
    {
        /// <summary>
        /// 运行动作方法。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="action">给定的动作方法。</param>
        public static void RunActionOrCancellation(this CancellationToken cancellationToken, Action action)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            action.Invoke();
        }

        /// <summary>
        /// 异步运行动作方法。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="action">给定的动作方法。</param>
        /// <returns>返回一个异步操作。</returns>
        public static Task RunActionOrCancellationAsync(this CancellationToken cancellationToken, Action action)
        {
            //cancellationToken.ThrowIfSourceDisposed();
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);

            return Task.Run(action, cancellationToken);
        }

        /// <summary>
        /// 异步运行动作方法。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="action">给定的动作方法。</param>
        /// <returns>返回一个异步操作。</returns>
        public static ValueTask RunActionOrCancellationValueAsync(this CancellationToken cancellationToken, Action action)
            => new ValueTask(cancellationToken.RunActionOrCancellationAsync(action));


        /// <summary>
        /// 运行工厂方法。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="factory">给定的工厂方法。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        public static TResult RunFactoryOrCancellation<TResult>(this CancellationToken cancellationToken, Func<TResult> factory)
        {
            if (cancellationToken.IsCancellationRequested)
                return default;

            return factory.Invoke();
        }

        /// <summary>
        /// 异步运行工厂方法。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="factory">给定的工厂方法。</param>
        /// <returns>返回一个包含 <typeparamref name="TResult"/> 的异步操作。</returns>
        public static Task<TResult> RunFactoryOrCancellationAsync<TResult>(this CancellationToken cancellationToken, Func<TResult> factory)
        {
            //cancellationToken.ThrowIfSourceDisposed();
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled<TResult>(cancellationToken);

            return Task.Run(factory, cancellationToken);
        }

        /// <summary>
        /// 异步运行工厂方法。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="factory">给定的工厂方法。</param>
        /// <returns>返回一个包含 <typeparamref name="TResult"/> 的异步操作。</returns>
        public static ValueTask<TResult> RunFactoryOrCancellationValueAsync<TResult>(this CancellationToken cancellationToken, Func<TResult> factory)
            => new ValueTask<TResult>(cancellationToken.RunFactoryOrCancellationAsync(factory));
    }
}
