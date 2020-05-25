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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 更新接口。
    /// </summary>
    /// <typeparam name="TUpdatedBy">指定的更新者。</typeparam>
    /// <typeparam name="TUpdatedTime">指定的更新时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
    public interface IUpdation<TUpdatedBy, TUpdatedTime> : ICreation<TUpdatedBy, TUpdatedTime>, IUpdation
        where TUpdatedBy : IEquatable<TUpdatedBy>
        where TUpdatedTime : struct
    {
        /// <summary>
        /// 更新时间。
        /// </summary>
        TUpdatedTime UpdatedTime { get; set; }

        /// <summary>
        /// 更新者。
        /// </summary>
        TUpdatedBy UpdatedBy { get; set; }


        /// <summary>
        /// 异步获取更新者。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TUpdatedBy"/> （兼容标识或字符串）的异步操作。</returns>
        new Task<TUpdatedBy> GetUpdatedByAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取更新时间。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TUpdatedTime"/> （兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        new Task<TUpdatedTime> GetUpdatedTimeAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 异步设置更新者。
        /// </summary>
        /// <param name="createdBy">给定的 <typeparamref name="TUpdatedBy"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task SetUpdatedByAsync(TUpdatedBy createdBy, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步设置更新时间。
        /// </summary>
        /// <param name="createdTime">给定的 <typeparamref name="TUpdatedTime"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task SetUpdatedTimeAsync(TUpdatedTime createdTime, CancellationToken cancellationToken = default);
    }


    /// <summary>
    /// 更新接口。
    /// </summary>
    public interface IUpdation : ICreation
    {
        /// <summary>
        /// 异步获取更新者。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含更新者（兼容标识或字符串）的异步操作。</returns>
        Task<object> GetUpdatedByAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取更新时间。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        Task<object> GetUpdatedTimeAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 异步设置更新者。
        /// </summary>
        /// <param name="obj">给定的更新者对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task SetUpdatedByAsync(object obj, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步设置更新时间。
        /// </summary>
        /// <param name="obj">给定的更新时间对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task SetUpdatedTimeAsync(object obj, CancellationToken cancellationToken = default);
    }
}