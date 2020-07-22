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
    /// 数据存储标识符生成器接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    public interface IDataStoreIdentityGenerator<TId> : IStoreIdentityGenerator
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 生成审计标识。
        /// </summary>
        /// <returns>返回 <typeparamref name="TId"/>。</returns>
        TId GenerateAuditId();

        /// <summary>
        /// 异步生成审计标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        Task<TId> GenerateAuditIdAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 生成迁移标识。
        /// </summary>
        /// <returns>返回 <typeparamref name="TId"/>。</returns>
        TId GenerateMigrationId();

        /// <summary>
        /// 异步生成迁移标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        Task<TId> GenerateMigrationIdAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 生成实体标识。
        /// </summary>
        /// <returns>返回 <typeparamref name="TId"/>。</returns>
        TId GenerateTabulationId();

        /// <summary>
        /// 异步生成实体标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        Task<TId> GenerateTabulationIdAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 生成租户标识。
        /// </summary>
        /// <returns>返回 <typeparamref name="TId"/>。</returns>
        TId GenerateTenantId();

        /// <summary>
        /// 异步生成租户标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        Task<TId> GenerateTenantIdAsync(CancellationToken cancellationToken = default);
    }
}
