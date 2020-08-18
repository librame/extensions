#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Core.Identifiers;
    using Core.Services;

    /// <summary>
    /// 抽象数据存储标识生成器。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    public abstract class AbstractDataStoreIdentificationGenerator<TId> : AbstractStoreIdentificationGenerator,
        IDataStoreIdentificationGenerator<TId>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractDataStoreIdentificationGenerator{TId}"/>。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="factory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractDataStoreIdentificationGenerator(IClockService clock,
            IIdentificationGeneratorFactory factory, ILoggerFactory loggerFactory)
            : base(clock, factory, loggerFactory)
        {
        }


        /// <summary>
        /// 生成标识。
        /// </summary>
        /// <param name="idName">给定的标识名称。</param>
        /// <returns>返回 <typeparamref name="TId"/>。</returns>
        public virtual TId GenerateId(string idName)
            => GenerateId<TId>(idName);

        /// <summary>
        /// 异步生成标识。
        /// </summary>
        /// <param name="idName">给定的标识名称。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public virtual Task<TId> GenerateIdAsync(string idName, CancellationToken cancellationToken = default)
            => GenerateIdAsync<TId>(idName, cancellationToken);


        /// <summary>
        /// 生成审计标识。
        /// </summary>
        /// <returns>返回 <typeparamref name="TId"/>。</returns>
        public virtual TId GenerateAuditId()
            => GenerateId("AuditId");

        /// <summary>
        /// 异步生成审计标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public virtual Task<TId> GenerateAuditIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("AuditId", cancellationToken);


        /// <summary>
        /// 生成迁移标识。
        /// </summary>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public virtual TId GenerateMigrationId()
            => GenerateId("MigrationId");

        /// <summary>
        /// 异步生成迁移标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public virtual Task<TId> GenerateMigrationIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("MigrationId", cancellationToken);


        /// <summary>
        /// 生成表格标识。
        /// </summary>
        /// <returns>返回 <typeparamref name="TId"/>。</returns>
        public virtual TId GenerateTabulationId()
            => GenerateId("TabulationId");

        /// <summary>
        /// 异步生成表格标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public virtual Task<TId> GenerateTabulationIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("TabulationId", cancellationToken);


        /// <summary>
        /// 生成租户标识。
        /// </summary>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public virtual TId GenerateTenantId()
            => GenerateId("TenantId");

        /// <summary>
        /// 异步生成租户标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public virtual Task<TId> GenerateTenantIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("TenantId", cancellationToken);
    }
}
