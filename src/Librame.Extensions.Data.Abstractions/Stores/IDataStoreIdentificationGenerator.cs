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
    /// 数据存储标识生成器接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    public interface IDataStoreIdentificationGenerator<TId> : IStoreIdentificationGenerator
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 生成标识。
        /// </summary>
        /// <param name="idName">给定的标识名称。</param>
        /// <returns>返回 <typeparamref name="TId"/>。</returns>
        TId GenerateId(string idName);

        /// <summary>
        /// 异步生成标识。
        /// </summary>
        /// <param name="idName">给定的标识名称。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        Task<TId> GenerateIdAsync(string idName, CancellationToken cancellationToken = default);


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
