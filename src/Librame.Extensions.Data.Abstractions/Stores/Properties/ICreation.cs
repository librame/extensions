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

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 创建接口。
    /// </summary>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    /// <typeparam name="TCreatedTime">指定的创建时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
    public interface ICreation<TCreatedBy, TCreatedTime> : ICreation
        where TCreatedBy : IEquatable<TCreatedBy>
        where TCreatedTime : struct
    {
        /// <summary>
        /// 创建时间。
        /// </summary>
        TCreatedTime CreatedTime { get; set; }

        /// <summary>
        /// 创建者。
        /// </summary>
        TCreatedBy CreatedBy { get; set; }


        /// <summary>
        /// 异步获取创建者。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TCreatedBy"/> （兼容标识或字符串）的异步操作。</returns>
        new Task<TCreatedBy> GetCreatedByAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取创建时间。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TCreatedTime"/> （兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        new Task<TCreatedTime> GetCreatedTimeAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 异步设置创建者。
        /// </summary>
        /// <param name="createdBy">给定的 <typeparamref name="TCreatedBy"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task SetCreatedByAsync(TCreatedBy createdBy, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步设置创建时间。
        /// </summary>
        /// <param name="createdTime">给定的 <typeparamref name="TCreatedTime"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task SetCreatedTimeAsync(TCreatedTime createdTime, CancellationToken cancellationToken = default);
    }


    /// <summary>
    /// 创建接口。
    /// </summary>
    public interface ICreation
    {
        /// <summary>
        /// 异步获取创建者。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含创建者（兼容标识或字符串）的异步操作。</returns>
        Task<object> GetCreatedByAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取创建时间。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        Task<object> GetCreatedTimeAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 异步设置创建者。
        /// </summary>
        /// <param name="obj">给定的创建者对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task SetCreatedByAsync(object obj, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步设置创建时间。
        /// </summary>
        /// <param name="obj">给定的创建时间对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task SetCreatedTimeAsync(object obj, CancellationToken cancellationToken = default);
    }
}