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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 操作结果。
    /// </summary>
    public class OperationResult
    {
        private OperationResult(bool succeeded = true)
        {
            Succeeded = succeeded;
        }

        private OperationResult(IReadOnlyList<OperationError> errors)
        {
            Errors = errors.NotNull(nameof(errors));
        }


        /// <summary>
        /// 操作是否成功。
        /// </summary>
        public bool Succeeded { get; }

        /// <summary>
        /// 实体错误集合。
        /// </summary>
        /// <value>返回 <see cref="IReadOnlyList{OperationError}"/>。</value>
        public IReadOnlyList<OperationError> Errors { get; }


        /// <summary>
        /// 转换为字符串形式。
        /// </summary>
        /// <remarks>
        /// 如果操作成功将返回“Succeeded”，反之则返回一个逗号分隔的错误代码列表字符串。
        /// </remarks>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Succeeded
            ? "Succeeded"
            : string.Format(CultureInfo.InvariantCulture,
                "{0} : {1}",
                "Failed",
                string.Join(",", Errors.Select(x => x.Code).ToList()));


        /// <summary>
        /// 操作成功。
        /// </summary>
        /// <value>返回 <see cref="OperationResult"/>。</value>
        public static readonly OperationResult Success
            = new OperationResult();

        /// <summary>
        /// 表示失败的操作结果。
        /// </summary>
        /// <param name="errors">给定的 <see cref="OperationError"/> 数组。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public static OperationResult Failed(params OperationError[] errors)
            => new OperationResult(errors);

        /// <summary>
        /// 表示失败的操作结果。
        /// </summary>
        /// <typeparam name="TException">指定的异常类型。</typeparam>
        /// <param name="exceptions">给定的 <typeparamref name="TException"/>。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public static OperationResult Failed<TException>(params TException[] exceptions)
            where TException : Exception
            => new OperationResult(exceptions.Select(ex => OperationError.ToError(ex)).ToList());


        #region TryRun

        /// <summary>
        /// 尝试运行动作方法。
        /// </summary>
        /// <param name="action">给定的动作方法。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public static OperationResult TryRun(Action action)
            => TryRun<Exception>(action);

        /// <summary>
        /// 尝试运行动作方法。
        /// </summary>
        /// <typeparam name="TException">指定的异常类型。</typeparam>
        /// <param name="action">给定的动作方法。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public static OperationResult TryRun<TException>(Action action)
            where TException : Exception
        {
            try
            {
                action?.Invoke();
                return Success;
            }
            catch (TException ex)
            {
                return Failed(ex);
            }
        }


        /// <summary>
        /// 尝试运行工厂方法。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="valueFactory">给定的值工厂方法。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public static OperationResult TryRun<TValue>(Func<TValue> valueFactory)
            => TryRun<TValue, Exception>(valueFactory, out _);

        /// <summary>
        /// 尝试运行工厂方法。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <typeparam name="TException">指定的异常类型。</typeparam>
        /// <param name="valueFactory">给定的值工厂方法。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public static OperationResult TryRun<TValue, TException>(Func<TValue> valueFactory)
            where TException : Exception
            => TryRun(valueFactory, out _);

        /// <summary>
        /// 尝试运行工厂方法。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="valueFactory">给定的值工厂方法。</param>
        /// <param name="value">输出值。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public static OperationResult TryRun<TValue>
            (Func<TValue> valueFactory, out TValue value)
            => TryRun<TValue, Exception>(valueFactory, out value);

        /// <summary>
        /// 尝试运行工厂方法。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <typeparam name="TException">指定的异常类型。</typeparam>
        /// <param name="valueFactory">给定的值工厂方法。</param>
        /// <param name="value">输出值。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static OperationResult TryRun<TValue, TException>
            (Func<TValue> valueFactory, out TValue value)
            where TException : Exception
        {
            valueFactory.NotNull(nameof(valueFactory));

            try
            {
                value = valueFactory.Invoke();
                return Success;
            }
            catch (TException ex)
            {
                value = default;
                return Failed(ex);
            }
        }


        /// <summary>
        /// 异步尝试运行异步工厂方法。
        /// </summary>
        /// <param name="taskFactory">给定的异步操作工厂方法。</param>
        /// <returns>返回一个包含 <see cref="OperationResult"/> 的异步操作。</returns>
        public static Task<OperationResult> TryRunAsync(Func<Task> taskFactory)
            => TryRunAsync<Exception>(taskFactory);

        /// <summary>
        /// 异步尝试运行异步工厂方法。
        /// </summary>
        /// <typeparam name="TException">指定的异常类型。</typeparam>
        /// <param name="taskFactory">给定的异步操作工厂方法。</param>
        /// <returns>返回一个包含 <see cref="OperationResult"/> 的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static async Task<OperationResult> TryRunAsync<TException>(Func<Task> taskFactory)
            where TException : Exception
        {
            taskFactory.NotNull(nameof(taskFactory));

            try
            {
                await taskFactory.Invoke().ConfigureAwait();
                return Success;
            }
            catch (TException ex)
            {
                return Failed(ex);
            }
        }


        /// <summary>
        /// 异步尝试运行异步工厂方法。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="taskFactory">给定的异步操作工厂方法。</param>
        /// <returns>返回一个包含 <see cref="OperationResult"/> 的异步操作。</returns>
        public static Task<OperationResult> TryRunAsync<TValue>(Func<Task<TValue>> taskFactory)
            => TryRunAsync<TValue, Exception>(taskFactory);

        /// <summary>
        /// 异步尝试运行异步工厂方法。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <typeparam name="TException">指定的异常类型。</typeparam>
        /// <param name="taskFactory">给定的异步操作工厂方法。</param>
        /// <returns>返回一个包含 <see cref="OperationResult"/> 的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static async Task<OperationResult> TryRunAsync<TValue, TException>(Func<Task<TValue>> taskFactory)
            where TException : Exception
        {
            taskFactory.NotNull(nameof(taskFactory));

            try
            {
                _ = await taskFactory.Invoke().ConfigureAwait();
                return Success;
            }
            catch (TException ex)
            {
                return Failed(ex);
            }
        }


        /// <summary>
        /// 异步尝试运行异步工厂方法。
        /// </summary>
        /// <param name="valueTaskFactory">给定的异步操作工厂方法。</param>
        /// <returns>返回一个包含 <see cref="OperationResult"/> 的异步操作。</returns>
        public static ValueTask<OperationResult> TryRunValueAsync(Func<ValueTask> valueTaskFactory)
            => TryRunValueAsync<Exception>(valueTaskFactory);

        /// <summary>
        /// 异步尝试运行异步工厂方法。
        /// </summary>
        /// <typeparam name="TException">指定的异常类型。</typeparam>
        /// <param name="valueTaskFactory">给定的异步操作工厂方法。</param>
        /// <returns>返回一个包含 <see cref="OperationResult"/> 的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static async ValueTask<OperationResult> TryRunValueAsync<TException>(Func<ValueTask> valueTaskFactory)
            where TException : Exception
        {
            valueTaskFactory.NotNull(nameof(valueTaskFactory));

            try
            {
                await valueTaskFactory.Invoke().ConfigureAwait();
                return Success;
            }
            catch (TException ex)
            {
                return Failed(ex);
            }
        }


        /// <summary>
        /// 异步尝试运行异步工厂方法。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="valueTaskFactory">给定的异步操作工厂方法。</param>
        /// <returns>返回一个包含 <see cref="OperationResult"/> 的异步操作。</returns>
        public static Task<OperationResult> TryRunValueAsync<TValue>(Func<ValueTask<TValue>> valueTaskFactory)
            => TryRunValueAsync<TValue, Exception>(valueTaskFactory);

        /// <summary>
        /// 异步尝试运行异步工厂方法。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <typeparam name="TException">指定的异常类型。</typeparam>
        /// <param name="valueTaskFactory">给定的异步操作工厂方法。</param>
        /// <returns>返回一个包含 <see cref="OperationResult"/> 的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static async Task<OperationResult> TryRunValueAsync<TValue, TException>(Func<ValueTask<TValue>> valueTaskFactory)
            where TException : Exception
        {
            valueTaskFactory.NotNull(nameof(valueTaskFactory));

            try
            {
                _ = await valueTaskFactory.Invoke().ConfigureAwait();
                return Success;
            }
            catch (TException ex)
            {
                return Failed(ex);
            }
        }

        #endregion

    }
}
