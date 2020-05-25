#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Resources;

    /// <summary>
    /// 抽象状态。
    /// </summary>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    public abstract class AbstractStatus<TStatus> : IStatus<TStatus>
        where TStatus : struct
    {
        /// <summary>
        /// 状态。
        /// </summary>
        [Display(Name = nameof(Status), GroupName = "DataGroup", ResourceType = typeof(AbstractEntityResource))]
        public virtual TStatus Status { get; set; }


        /// <summary>
        /// 获取状态。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task{TStatus}"/>。</returns>
        public Task<TStatus> GetStatusAsync(CancellationToken cancellationToken = default)
            => cancellationToken.RunFactoryOrCancellationAsync(() => Status);

        Task<object> IStatus.GetStatusAsync(CancellationToken cancellationToken)
            => cancellationToken.RunFactoryOrCancellationAsync(() => (object)Status);


        /// <summary>
        /// 设置状态。
        /// </summary>
        /// <param name="status">给定的 <typeparamref name="TStatus"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task SetStatusAsync(TStatus status, CancellationToken cancellationToken = default)
            => cancellationToken.RunActionOrCancellationAsync(() => Status = status);

        /// <summary>
        /// 设置状态。
        /// </summary>
        /// <param name="obj">给定的状态对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task SetStatusAsync(object obj, CancellationToken cancellationToken = default)
        {
            var status = obj.CastTo<object, TStatus>(nameof(obj));

            return cancellationToken.RunActionOrCancellationAsync(() => Status = status);
        }

    }
}
