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
using System.Linq;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 实体结果。
    /// </summary>
    public class EntityResult
    {
        private List<EntityError> _errors = new List<EntityError>();


        /// <summary>
        /// 操作是否成功。
        /// </summary>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// 实体错误集合。
        /// </summary>
        /// <value>返回 <see cref="IEnumerable{EntityError}"/>。</value>
        public IEnumerable<EntityError> Errors => _errors;


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
            : string.Format("{0} : {1}", "Failed", string.Join(",", Errors.Select(x => x.Code).ToList()));


        /// <summary>
        /// 表示操作成功的实体结果。
        /// </summary>
        /// <returns>返回 <see cref="EntityResult"/>。</returns>
        public static EntityResult Success { get; }
            = new EntityResult { Succeeded = true };


        /// <summary>
        /// 创建操作失败的实体结果。
        /// </summary>
        /// <param name="errors">给定的 <see cref="EntityResult"/>。</param>
        /// <returns>返回 <see cref="EntityResult"/>。</returns>
        public static EntityResult Failed(params EntityError[] errors)
        {
            var result = new EntityResult { Succeeded = false };

            if (errors.IsNotEmpty())
                result._errors.AddRange(errors);

            return result;
        }

        /// <summary>
        /// 创建操作失败的实体结果。
        /// </summary>
        /// <param name="exception">给定的 <see cref="Exception"/>。</param>
        /// <returns>返回 <see cref="EntityResult"/>。</returns>
        public static EntityResult Failed(Exception exception)
            => Failed(EntityError.ToError(exception));


        /// <summary>
        /// 异步尝试运行工厂方法。
        /// </summary>
        /// <param name="taskFactory">给定的异步操作工厂方法。</param>
        /// <returns>返回一个包含 <see cref="EntityResult"/> 的异步操作。</returns>
        public static async Task<EntityResult> TryRunFactoryAsync(Func<Task> taskFactory)
        {
            taskFactory.NotNull(nameof(taskFactory));

            try
            {
                await taskFactory.Invoke().ConfigureAndWaitAsync();
                return Success;
            }
            catch (Exception ex)
            {
                return Failed(ex);
            }
        }

        /// <summary>
        /// 尝试运行动作方法。
        /// </summary>
        /// <param name="action">给定的动作方法。</param>
        /// <returns>返回 <see cref="EntityResult"/>。</returns>
        public static EntityResult TryRunAction(Action action)
        {
            try
            {
                action?.Invoke();
                return Success;
            }
            catch (Exception ex)
            {
                return Failed(ex);
            }
        }

    }
}
