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
        private List<OperationError> _errors = new List<OperationError>();


        /// <summary>
        /// 操作是否成功。
        /// </summary>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// 实体错误集合。
        /// </summary>
        /// <value>返回 <see cref="IReadOnlyList{OperationError}"/>。</value>
        public IReadOnlyList<OperationError> Errors
            => _errors.AsReadOnlyList();


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
            : string.Format(CultureInfo.InvariantCulture, "{0} : {1}", "Failed", string.Join(",", Errors.Select(x => x.Code).ToList()));


        /// <summary>
        /// 表示成功的操作结果。
        /// </summary>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public static OperationResult Success { get; }
            = new OperationResult { Succeeded = true };


        /// <summary>
        /// 表示失败的操作结果。
        /// </summary>
        /// <param name="errors">给定的 <see cref="OperationError"/> 数组。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public static OperationResult Failed(params OperationError[] errors)
        {
            var result = new OperationResult { Succeeded = false };

            if (errors.IsNotEmpty())
                result._errors.AddRange(errors);

            return result;
        }

        /// <summary>
        /// 表示失败的操作结果。
        /// </summary>
        /// <typeparam name="TException">指定的异常类型。</typeparam>
        /// <param name="exception">给定的 <typeparamref name="TException"/>。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public static OperationResult Failed<TException>(TException exception)
            where TException : Exception
            => Failed(OperationError.ToError(exception));


        /// <summary>
        /// 异步尝试运行工厂方法。
        /// </summary>
        /// <param name="taskFactory">给定的异步操作工厂方法。</param>
        /// <returns>返回一个包含 <see cref="OperationResult"/> 的异步操作。</returns>
        public static Task<OperationResult> TryRunFactoryAsync(Func<Task> taskFactory)
            => TryRunFactoryAsync<Exception>(taskFactory);

        /// <summary>
        /// 异步尝试运行工厂方法。
        /// </summary>
        /// <typeparam name="TException">指定的异常类型。</typeparam>
        /// <param name="taskFactory">给定的异步操作工厂方法。</param>
        /// <returns>返回一个包含 <see cref="OperationResult"/> 的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static async Task<OperationResult> TryRunFactoryAsync<TException>(Func<Task> taskFactory)
            where TException : Exception
        {
            taskFactory.NotNull(nameof(taskFactory));

            try
            {
                await taskFactory.Invoke().ConfigureAndWaitAsync();
                return Success;
            }
            catch (TException ex)
            {
                return Failed(ex);
            }
        }


        /// <summary>
        /// 尝试运行动作方法。
        /// </summary>
        /// <param name="action">给定的动作方法。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public static OperationResult TryRunAction(Action action)
            => TryRunAction<Exception>(action);

        /// <summary>
        /// 尝试运行动作方法。
        /// </summary>
        /// <typeparam name="TException">指定的异常类型。</typeparam>
        /// <param name="action">给定的动作方法。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public static OperationResult TryRunAction<TException>(Action action)
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

    }
}
