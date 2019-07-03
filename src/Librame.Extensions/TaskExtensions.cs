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
    /// 任务静态扩展。
    /// </summary>
    public static class TaskExtensions
    {
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

    }
}
