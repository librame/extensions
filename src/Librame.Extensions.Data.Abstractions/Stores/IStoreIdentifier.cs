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
    using Core.Identifiers;
    using Core.Services;

    /// <summary>
    /// 存储标识符接口。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    public interface IStoreIdentifier<TGenId> : IStoreIdentifier
        where TGenId : IEquatable<TGenId>
    {
        /// <summary>
        /// 标识符生成器。
        /// </summary>
        /// <value>返回 <see cref="IIdentifierGenerator{TIdentifier}"/>。</value>
        IIdentifierGenerator<TGenId> Generator { get; }


        /// <summary>
        /// 异步获取审计标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TGenId"/> 的异步操作。</returns>
        new Task<TGenId> GetAuditIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取实体标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TGenId"/> 的异步操作。</returns>
        new Task<TGenId> GetEntityIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取迁移标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TGenId"/> 的异步操作。</returns>
        new Task<TGenId> GetMigrationIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取租户标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TGenId"/> 的异步操作。</returns>
        new Task<TGenId> GetTenantIdAsync(CancellationToken cancellationToken = default);
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


        /// <summary>
        /// 异步获取审计标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识对象的异步操作。</returns>
        Task<object> GetAuditIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取实体标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识对象的异步操作。</returns>
        Task<object> GetEntityIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取迁移标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识对象的异步操作。</returns>
        Task<object> GetMigrationIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取租户标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识对象的异步操作。</returns>
        Task<object> GetTenantIdAsync(CancellationToken cancellationToken = default);
    }
}
