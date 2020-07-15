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
        /// 异步运行可取消的动作方法（如果已请求取消此令牌，那么将可选抛出异常）。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="action">给定的动作方法。</param>
        /// <param name="throwIfCancellationRequested">如果已请求取消此令牌是否抛出异常。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public static Task RunOrCancelAsync(this CancellationToken cancellationToken,
            Action action, bool throwIfCancellationRequested = true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                if (!cancellationToken.CanBeCanceled)
                {
                    if (throwIfCancellationRequested)
                        cancellationToken.ThrowIfCancellationRequested();

                    return Task.CompletedTask;
                }

                if (cancellationToken.CanBeCanceled)
                    return Task.FromCanceled(cancellationToken);
            }

            return Task.Run(action, cancellationToken);
        }

        /// <summary>
        /// 异步运行动作方法。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="action">给定的动作方法。</param>
        /// <param name="throwIfCancellationRequested">如果已请求取消此令牌是否抛出异常。</param>
        /// <returns>返回 <see cref="ValueTask"/>。</returns>
        public static ValueTask RunOrCancelValueAsync(this CancellationToken cancellationToken,
            Action action, bool throwIfCancellationRequested = true)
            => new ValueTask(cancellationToken.RunOrCancelAsync(action, throwIfCancellationRequested));


        /// <summary>
        /// 异步运行工厂方法。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="func">给定的工厂方法。</param>
        /// <param name="throwIfCancellationRequested">如果已请求取消此令牌是否抛出异常。</param>
        /// <returns>返回一个包含 <typeparamref name="TResult"/> 的异步操作。</returns>
        public static Task<TResult> RunOrCancelAsync<TResult>(this CancellationToken cancellationToken,
            Func<TResult> func, bool throwIfCancellationRequested = true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                if (!cancellationToken.CanBeCanceled)
                {
                    if (throwIfCancellationRequested)
                        cancellationToken.ThrowIfCancellationRequested();

                    return Task.FromResult<TResult>(default);
                }

                if (cancellationToken.CanBeCanceled)
                    return Task.FromCanceled<TResult>(cancellationToken);
            }

            return Task.Run(func, cancellationToken);
        }

        /// <summary>
        /// 异步运行异步工厂方法。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="func">给定的异步工厂方法。</param>
        /// <param name="throwIfCancellationRequested">如果已请求取消此令牌是否抛出异常。</param>
        /// <returns>返回一个包含 <typeparamref name="TResult"/> 的异步操作。</returns>
        public static Task<TResult> RunOrCancelAsync<TResult>(this CancellationToken cancellationToken,
            Func<Task<TResult>> func, bool throwIfCancellationRequested = true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                if (!cancellationToken.CanBeCanceled)
                {
                    if (throwIfCancellationRequested)
                        cancellationToken.ThrowIfCancellationRequested();

                    return Task.FromResult<TResult>(default);
                }

                if (cancellationToken.CanBeCanceled)
                    return Task.FromCanceled<TResult>(cancellationToken);
            }
            
            return Task.Run(func, cancellationToken);
        }


        /// <summary>
        /// 异步运行工厂方法。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="func">给定的工厂方法。</param>
        /// <param name="throwIfCancellationRequested">如果已请求取消此令牌是否抛出异常。</param>
        /// <returns>返回一个包含 <typeparamref name="TResult"/> 的异步操作。</returns>
        public static ValueTask<TResult> RunOrCancelValueAsync<TResult>(this CancellationToken cancellationToken,
            Func<TResult> func, bool throwIfCancellationRequested = true)
            => new ValueTask<TResult>(cancellationToken.RunOrCancelAsync(func, throwIfCancellationRequested));

        /// <summary>
        /// 异步运行异步工厂方法。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="func">给定的异步工厂方法。</param>
        /// <param name="throwIfCancellationRequested">如果已请求取消此令牌是否抛出异常。</param>
        /// <returns>返回一个包含 <typeparamref name="TResult"/> 的异步操作。</returns>
        public static ValueTask<TResult> RunOrCancelValueAsync<TResult>(this CancellationToken cancellationToken,
            Func<Task<TResult>> func, bool throwIfCancellationRequested = true)
            => new ValueTask<TResult>(cancellationToken.RunOrCancelAsync(func, throwIfCancellationRequested));

        #endregion


        #region ConfiguredTaskAwaitable

        /// <summary>
        /// 等待已配置的任务。
        /// </summary>
        /// <param name="task">给定的 <see cref="Task"/>。</param>
        /// <returns>返回 <see cref="ConfiguredTaskAwaitable"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static ConfiguredTaskAwaitable ConfigureAwait(this Task task)
            => task.NotNull(nameof(task)).ConfigureAwait(false); // 统一使用 false 可避免不必要的封送处理以提高性能

        /// <summary>
        /// 等待存在结果的已配置任务。
        /// </summary>
        /// <typeparam name="TResult">指定的类型。</typeparam>
        /// <param name="task">给定的 <see cref="Task{TResult}"/>。</param>
        /// <returns>返回 <see cref="ConfiguredTaskAwaitable{TResult}"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static ConfiguredTaskAwaitable<TResult> ConfigureAwait<TResult>(this Task<TResult> task)
            => task.NotNull(nameof(task)).ConfigureAwait(false); // 统一使用 false 可避免不必要的封送处理以提高性能


        /// <summary>
        /// 结束对已完成任务的等待。
        /// </summary>
        /// <param name="task">给定的 <see cref="Task"/>。</param>
        public static void ConfigureAwaitCompleted(this Task task)
            => task.ConfigureAwait().GetAwaiter().GetResult();

        /// <summary>
        /// 结束对存在结果的已完成任务的等待。
        /// </summary>
        /// <typeparam name="TResult">指定的类型。</typeparam>
        /// <param name="task">给定的 <see cref="Task{TResult}"/>。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        public static TResult ConfigureAwaitCompleted<TResult>(this Task<TResult> task)
            => task.ConfigureAwait().GetAwaiter().GetResult();


        /// <summary>
        /// 等待已配置的任务。
        /// </summary>
        /// <param name="valueTask">给定的 <see cref="ValueTask"/>。</param>
        /// <returns>返回 <see cref="ConfiguredValueTaskAwaitable"/>。</returns>
        public static ConfiguredValueTaskAwaitable ConfigureAwait(this ValueTask valueTask)
            => valueTask.ConfigureAwait(false); // 统一使用 false 可避免不必要的封送处理以提高性能

        /// <summary>
        /// 等待存在结果的已配置任务。
        /// </summary>
        /// <typeparam name="TResult">指定的类型。</typeparam>
        /// <param name="valueTask">给定的 <see cref="ValueTask{TResult}"/>。</param>
        /// <returns>返回 <see cref="ConfiguredValueTaskAwaitable{TResult}"/>。</returns>
        public static ConfiguredValueTaskAwaitable<TResult> ConfigureAwait<TResult>
            (this ValueTask<TResult> valueTask)
            => valueTask.ConfigureAwait(false); // 统一使用 false 可避免不必要的封送处理以提高性能


        /// <summary>
        /// 结束对已完成任务的等待。
        /// </summary>
        /// <param name="valueTask">给定的 <see cref="ValueTask"/>。</param>
        public static void ConfigureAwaitCompleted(this ValueTask valueTask)
            => valueTask.ConfigureAwait().GetAwaiter().GetResult();

        /// <summary>
        /// 结束对存在结果的已完成任务的等待。
        /// </summary>
        /// <typeparam name="TResult">指定的类型。</typeparam>
        /// <param name="valueTask">给定的 <see cref="ValueTask{TResult}"/>。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        public static TResult ConfigureAwaitCompleted<TResult>(this ValueTask<TResult> valueTask)
            => valueTask.ConfigureAwait().GetAwaiter().GetResult();

        #endregion

    }
}
