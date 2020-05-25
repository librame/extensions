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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions
{
    /// <summary>
    /// 任务静态扩展。
    /// </summary>
    public static class TaskExtensions
    {

        #region CancellationToken

        /// <summary>
        /// 异步运行动作方法。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="action">给定的动作方法。</param>
        /// <returns>返回一个异步操作。</returns>
        public static Task RunActionOrCancellationAsync
            (this CancellationToken cancellationToken, Action action)
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
        public static ValueTask RunActionOrCancellationValueAsync
            (this CancellationToken cancellationToken, Action action)
            => new ValueTask(cancellationToken.RunActionOrCancellationAsync(action));

        /// <summary>
        /// 异步运行工厂方法。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="func">给定的工厂方法。</param>
        /// <returns>返回一个包含 <typeparamref name="TResult"/> 的异步操作。</returns>
        public static Task<TResult> RunFactoryOrCancellationAsync<TResult>
            (this CancellationToken cancellationToken, Func<TResult> func)
        {
            //cancellationToken.ThrowIfSourceDisposed();
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled<TResult>(cancellationToken);

            return Task.Run(func, cancellationToken);
        }

        /// <summary>
        /// 异步运行异步工厂方法。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="func">给定的异步工厂方法。</param>
        /// <returns>返回一个包含 <typeparamref name="TResult"/> 的异步操作。</returns>
        public static Task<TResult> RunFactoryOrCancellationAsync<TResult>
            (this CancellationToken cancellationToken, Func<Task<TResult>> func)
        {
            //cancellationToken.ThrowIfSourceDisposed();
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled<TResult>(cancellationToken);

            return Task.Run(func, cancellationToken);
        }

        /// <summary>
        /// 异步运行工厂方法。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="func">给定的工厂方法。</param>
        /// <returns>返回一个包含 <typeparamref name="TResult"/> 的异步操作。</returns>
        public static ValueTask<TResult> RunFactoryOrCancellationValueAsync<TResult>
            (this CancellationToken cancellationToken, Func<TResult> func)
            => new ValueTask<TResult>(cancellationToken.RunFactoryOrCancellationAsync(func));

        /// <summary>
        /// 异步运行工厂方法。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="func">给定的工厂方法。</param>
        /// <returns>返回一个包含 <typeparamref name="TResult"/> 的异步操作。</returns>
        public static ValueTask<TResult> RunFactoryOrCancellationValueAsync<TResult>
            (this CancellationToken cancellationToken, Func<Task<TResult>> func)
            => new ValueTask<TResult>(cancellationToken.RunFactoryOrCancellationAsync(func));

        #endregion


        #region Task

        /// <summary>
        /// 异步配置并等待。
        /// </summary>
        /// <param name="task">给定的 <see cref="Task"/>。</param>
        /// <returns>返回 <see cref="ConfiguredTaskAwaitable"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static ConfiguredTaskAwaitable ConfigureAndWaitAsync(this Task task)
            => task.NotNull(nameof(task)).ConfigureAwait(false);

        /// <summary>
        /// 异步配置并返回结果。
        /// </summary>
        /// <typeparam name="TResult">指定的类型。</typeparam>
        /// <param name="task">给定的 <see cref="Task{TResult}"/>。</param>
        /// <returns>返回 <see cref="ConfiguredTaskAwaitable{TResult}"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static ConfiguredTaskAwaitable<TResult> ConfigureAndResultAsync<TResult>(this Task<TResult> task)
            => task.NotNull(nameof(task)).ConfigureAwait(true);


        /// <summary>
        /// 配置并等待。
        /// </summary>
        /// <param name="task">给定的 <see cref="Task"/>。</param>
        public static void ConfigureAndWait(this Task task)
            => task.ConfigureAndWaitAsync().GetAwaiter().GetResult();

        /// <summary>
        /// 配置并返回结果。
        /// </summary>
        /// <typeparam name="TResult">指定的类型。</typeparam>
        /// <param name="task">给定的 <see cref="Task{TResult}"/>。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        public static TResult ConfigureAndResult<TResult>(this Task<TResult> task)
            => task.ConfigureAndResultAsync().GetAwaiter().GetResult();

        #endregion


        #region ValueTask

        /// <summary>
        /// 异步配置并等待。
        /// </summary>
        /// <param name="valueTask">给定的 <see cref="ValueTask"/>。</param>
        /// <returns>返回 <see cref="ConfiguredValueTaskAwaitable"/>。</returns>
        public static ConfiguredValueTaskAwaitable ConfigureAndWaitAsync(this ValueTask valueTask)
            => valueTask.ConfigureAwait(false);

        /// <summary>
        /// 异步配置并返回结果。
        /// </summary>
        /// <typeparam name="TResult">指定的类型。</typeparam>
        /// <param name="valueTask">给定的 <see cref="ValueTask{TResult}"/>。</param>
        /// <returns>返回 <see cref="ConfiguredValueTaskAwaitable{TResult}"/>。</returns>
        public static ConfiguredValueTaskAwaitable<TResult> ConfigureAndResultAsync<TResult>(this ValueTask<TResult> valueTask)
            => valueTask.ConfigureAwait(true);


        /// <summary>
        /// 配置并等待。
        /// </summary>
        /// <param name="valueTask">给定的 <see cref="ValueTask"/>。</param>
        public static void ConfigureAndWait(this ValueTask valueTask)
            => valueTask.ConfigureAndWaitAsync().GetAwaiter().GetResult();

        /// <summary>
        /// 配置并返回结果。
        /// </summary>
        /// <typeparam name="TResult">指定的类型。</typeparam>
        /// <param name="valueTask">给定的 <see cref="ValueTask{TResult}"/>。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        public static TResult ConfigureAndResult<TResult>(this ValueTask<TResult> valueTask)
            => valueTask.ConfigureAndResultAsync().GetAwaiter().GetResult();

        #endregion

    }
}
