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
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Threads
{
    /// <summary>
    /// 抽象内存锁定器静态扩展。
    /// </summary>
    public static class AbstractionMemoryLockerExtensions
    {
        /// <summary>
        /// 等待动作方法。
        /// </summary>
        /// <param name="locker">给定的 <see cref="IMemoryLocker"/>。</param>
        /// <param name="action">给定执行的动作方法。</param>
        /// <param name="cancellationToken">给定的取消令牌（可选）。</param>
        /// <param name="millisecondsTimeout">给定的毫秒超时数（可选）。</param>
        /// <param name="timeout">给定的超时（可选）。</param>
        /// <param name="releaseCount">给定要释放的线程数（可选）。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static void WaitAction(this IMemoryLocker locker,
            Action action, CancellationToken? cancellationToken = null,
            int? millisecondsTimeout = null, TimeSpan? timeout = null, int? releaseCount = null)
        {
            locker.NotNull(nameof(locker));
            action.NotNull(nameof(action));

            locker.Wait(millisecondsTimeout, timeout, cancellationToken);

            action.Invoke();

            locker.Release(releaseCount);
        }

        /// <summary>
        /// 异步等待动作方法。
        /// </summary>
        /// <param name="locker">给定的 <see cref="IMemoryLocker"/>。</param>
        /// <param name="factory">给定执行的工厂方法。</param>
        /// <param name="cancellationToken">给定的取消令牌（可选）。</param>
        /// <param name="millisecondsTimeout">给定的毫秒超时数（可选）。</param>
        /// <param name="timeout">给定的超时（可选）。</param>
        /// <param name="releaseCount">给定要释放的线程数（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static async Task WaitActionAsync(this IMemoryLocker locker,
            Func<Task> factory, CancellationToken? cancellationToken = null,
            int? millisecondsTimeout = null, TimeSpan? timeout = null, int? releaseCount = null)
        {
            locker.NotNull(nameof(locker));
            factory.NotNull(nameof(factory));

            await locker.WaitAsync(millisecondsTimeout, timeout, cancellationToken).ConfigureAndWaitAsync();

            await factory.Invoke().ConfigureAndWaitAsync();

            locker.Release(releaseCount);
        }


        /// <summary>
        /// 等待工厂方法。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="locker">给定的 <see cref="IMemoryLocker"/>。</param>
        /// <param name="factory">给定执行的工厂方法。</param>
        /// <param name="cancellationToken">给定的取消令牌（可选）。</param>
        /// <param name="millisecondsTimeout">给定的毫秒超时数（可选）。</param>
        /// <param name="timeout">给定的超时（可选）。</param>
        /// <param name="releaseCount">给定要释放的线程数（可选）。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static TResult WaitFactory<TResult>(this IMemoryLocker locker,
            Func<TResult> factory, CancellationToken? cancellationToken = null,
            int? millisecondsTimeout = null, TimeSpan? timeout = null, int? releaseCount = null)
        {
            locker.NotNull(nameof(locker));
            factory.NotNull(nameof(factory));

            locker.Wait(millisecondsTimeout, timeout, cancellationToken);

            var result = factory.Invoke();

            locker.Release(releaseCount);

            return result;
        }

        /// <summary>
        /// 异步等待工厂方法。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="locker">给定的 <see cref="IMemoryLocker"/>。</param>
        /// <param name="factory">给定执行的工厂方法。</param>
        /// <param name="cancellationToken">给定的取消令牌（可选）。</param>
        /// <param name="millisecondsTimeout">给定的毫秒超时数（可选）。</param>
        /// <param name="timeout">给定的超时（可选）。</param>
        /// <param name="releaseCount">给定要释放的线程数（可选）。</param>
        /// <returns>返回 <see cref="Task{TResult}"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static async Task<TResult> WaitFactoryAsync<TResult>(this IMemoryLocker locker,
            Func<Task<TResult>> factory, CancellationToken? cancellationToken = null,
            int? millisecondsTimeout = null, TimeSpan? timeout = null, int? releaseCount = null)
        {
            locker.NotNull(nameof(locker));
            factory.NotNull(nameof(factory));

            await locker.WaitAsync(millisecondsTimeout, timeout, cancellationToken).ConfigureAndWaitAsync();

            var result = await factory.Invoke().ConfigureAndResultAsync();

            locker.Release(releaseCount);

            return result;
        }


        /// <summary>
        /// 尝试捕获异常的等待动作方法。
        /// </summary>
        /// <typeparam name="TException">指定的异常类型。</typeparam>
        /// <param name="locker">给定的 <see cref="IMemoryLocker"/>。</param>
        /// <param name="tryAction">给定尝试执行的动作方法。</param>
        /// <param name="catchAction">给定异常捕获的动作方法。</param>
        /// <param name="cancellationToken">给定的取消令牌（可选）。</param>
        /// <param name="millisecondsTimeout">给定的毫秒超时数（可选）。</param>
        /// <param name="timeout">给定的超时（可选）。</param>
        /// <param name="releaseCount">给定要释放的线程数（可选）。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static void TryCatchWaitAction<TException>(this IMemoryLocker locker,
            Action tryAction, Action<TException> catchAction, CancellationToken? cancellationToken = null,
            int? millisecondsTimeout = null, TimeSpan? timeout = null, int? releaseCount = null)
            where TException : Exception
        {
            locker.NotNull(nameof(locker));
            tryAction.NotNull(nameof(tryAction));
            catchAction.NotNull(nameof(catchAction));

            locker.Wait(millisecondsTimeout, timeout, cancellationToken);

            try
            {
                tryAction.Invoke();
            }
            catch (TException ex)
            {
                catchAction.Invoke(ex);
            }
            finally
            {
                locker.Release(releaseCount);
            }
        }

        /// <summary>
        /// 异步尝试捕获异常的等待动作方法。
        /// </summary>
        /// <typeparam name="TException">指定的异常类型。</typeparam>
        /// <param name="locker">给定的 <see cref="IMemoryLocker"/>。</param>
        /// <param name="tryFactory">给定尝试执行的工厂方法。</param>
        /// <param name="catchAction">给定异常捕获的动作方法。</param>
        /// <param name="cancellationToken">给定的取消令牌（可选）。</param>
        /// <param name="millisecondsTimeout">给定的毫秒超时数（可选）。</param>
        /// <param name="timeout">给定的超时（可选）。</param>
        /// <param name="releaseCount">给定要释放的线程数（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static async Task TryCatchWaitActionAsync<TException>(this IMemoryLocker locker,
            Func<Task> tryFactory, Action<TException> catchAction, CancellationToken? cancellationToken = null,
            int? millisecondsTimeout = null, TimeSpan? timeout = null, int? releaseCount = null)
            where TException : Exception
        {
            locker.NotNull(nameof(locker));
            tryFactory.NotNull(nameof(tryFactory));
            catchAction.NotNull(nameof(catchAction));

            await locker.WaitAsync(millisecondsTimeout, timeout, cancellationToken).ConfigureAndWaitAsync();

            try
            {
                await tryFactory.Invoke().ConfigureAndWaitAsync();
            }
            catch (TException ex)
            {
                catchAction.Invoke(ex);
            }
            finally
            {
                locker.Release(releaseCount);
            }
        }


        /// <summary>
        /// 尝试捕获异常的等待工厂方法。
        /// </summary>
        /// <typeparam name="TException">指定的异常类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="locker">给定的 <see cref="IMemoryLocker"/>。</param>
        /// <param name="tryFactory">给定尝试执行的工厂方法。</param>
        /// <param name="catchFactory">给定异常捕获的工厂方法。</param>
        /// <param name="cancellationToken">给定的取消令牌（可选）。</param>
        /// <param name="millisecondsTimeout">给定的毫秒超时数（可选）。</param>
        /// <param name="timeout">给定的超时（可选）。</param>
        /// <param name="releaseCount">给定要释放的线程数（可选）。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static TResult TryCatchWaitFactory<TException, TResult>(this IMemoryLocker locker,
            Func<TResult> tryFactory, Func<TException, TResult> catchFactory, CancellationToken? cancellationToken = null,
            int? millisecondsTimeout = null, TimeSpan? timeout = null, int? releaseCount = null)
            where TException : Exception
        {
            locker.NotNull(nameof(locker));
            tryFactory.NotNull(nameof(tryFactory));
            catchFactory.NotNull(nameof(catchFactory));

            locker.Wait(millisecondsTimeout, timeout, cancellationToken);

            try
            {
                return tryFactory.Invoke();
            }
            catch (TException ex)
            {
                return catchFactory.Invoke(ex);
            }
            finally
            {
                locker.Release(releaseCount);
            }
        }

        /// <summary>
        /// 异步尝试捕获异常的等待工厂方法。
        /// </summary>
        /// <typeparam name="TException">指定的异常类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="locker">给定的 <see cref="IMemoryLocker"/>。</param>
        /// <param name="tryFactory">给定尝试执行的工厂方法。</param>
        /// <param name="catchFactory">给定异常捕获的工厂方法。</param>
        /// <param name="cancellationToken">给定的取消令牌（可选）。</param>
        /// <param name="millisecondsTimeout">给定的毫秒超时数（可选）。</param>
        /// <param name="timeout">给定的超时（可选）。</param>
        /// <param name="releaseCount">给定要释放的线程数（可选）。</param>
        /// <returns>返回 <see cref="Task{TResult}"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static async Task<TResult> TryCatchWaitFactoryAsync<TException, TResult>(this IMemoryLocker locker,
            Func<Task<TResult>> tryFactory, Func<TException, TResult> catchFactory, CancellationToken? cancellationToken = null,
            int? millisecondsTimeout = null, TimeSpan? timeout = null, int? releaseCount = null)
            where TException : Exception
        {
            locker.NotNull(nameof(locker));
            tryFactory.NotNull(nameof(tryFactory));
            catchFactory.NotNull(nameof(catchFactory));

            await locker.WaitAsync(millisecondsTimeout, timeout, cancellationToken).ConfigureAndWaitAsync();

            try
            {
                return await tryFactory.Invoke().ConfigureAndResultAsync();
            }
            catch (TException ex)
            {
                return catchFactory.Invoke(ex);
            }
            finally
            {
                locker.Release(releaseCount);
            }
        }

    }
}
