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
    using Core.Identifiers;

    /// <summary>
    /// 数据存储标识符生成器接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    public interface IDataStoreIdentifierGenerator<TId> : IStoreIdentifierGenerator
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 标识符生成器。
        /// </summary>
        /// <value>返回 <see cref="IIdentifierGenerator{TId}"/>。</value>
        IIdentifierGenerator<TId> Generator { get; }


        /// <summary>
        /// 异步生成标识。
        /// </summary>
        /// <param name="idName">给定的标识名称。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        Task<TId> GenerateIdAsync(string idName, CancellationToken cancellationToken = default);
        /// <summary>
        /// 异步生成审计标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        Task<TId> GenerateAuditIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步生成实体标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        Task<TId> GenerateEntityIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步生成迁移标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        Task<TId> GenerateMigrationIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步生成租户标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        Task<TId> GenerateTenantIdAsync(CancellationToken cancellationToken = default);
    }
}
