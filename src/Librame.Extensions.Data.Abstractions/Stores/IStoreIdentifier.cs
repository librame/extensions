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
    using Core.Services;

    /// <summary>
    /// 存储标识符接口。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    public interface IStoreIdentifier<TGenId> : IStoreIdentifier
        where TGenId : IEquatable<TGenId>
    {
        /// <summary>
        /// 异步获取审计标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        Task<TGenId> GetAuditIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取实体标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        Task<TGenId> GetEntityIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取迁移标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        Task<TGenId> GetMigrationIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取租户标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        Task<TGenId> GetTenantIdAsync(CancellationToken cancellationToken = default);
    }


    /// <summary>
    /// 存储标识符接口（主要用作标记）。
    /// </summary>
    public interface IStoreIdentifier : IService
    {
        /// <summary>
        /// 时钟。
        /// </summary>
        IClockService Clock { get; }
    }
}
