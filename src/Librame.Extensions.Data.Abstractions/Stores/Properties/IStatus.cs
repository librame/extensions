#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 状态接口。
    /// </summary>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    public interface IStatus<TStatus> : IStatus
        where TStatus : struct
    {
        /// <summary>
        /// 状态。
        /// </summary>
        TStatus Status { get; set; }


        /// <summary>
        /// 异步获取状态。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TStatus"/> 的异步操作。</returns>
        new Task<TStatus> GetStatusAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步设置状态。
        /// </summary>
        /// <param name="status">给定的 <typeparamref name="TStatus"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task SetStatusAsync(TStatus status, CancellationToken cancellationToken = default);
    }


    /// <summary>
    /// 状态接口。
    /// </summary>
    public interface IStatus
    {
        /// <summary>
        /// 异步获取状态。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含状态的异步操作。</returns>
        Task<object> GetStatusAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步设置状态。
        /// </summary>
        /// <param name="obj">给定的状态对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task SetStatusAsync(object obj, CancellationToken cancellationToken = default);
    }
}
