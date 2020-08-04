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
    public abstract class AbstractDataStoreIdentityGenerator<TId> : AbstractStoreIdentityGenerator,
        IDataStoreIdentityGenerator<TId>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractDataStoreIdentityGenerator{TId}"/>。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="factory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractDataStoreIdentityGenerator(IClockService clock,
            IIdentificationGeneratorFactory factory, ILoggerFactory loggerFactory)
            : base(clock, factory, loggerFactory)
        {
        }


        /// <summary>
        /// 生成审计标识。
        /// </summary>
        /// <returns>返回 <typeparamref name="TId"/>。</returns>
        public virtual TId GenerateAuditId()
            => GenerateId<TId>("AuditId");

        /// <summary>
        /// 异步生成审计标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public virtual Task<TId> GenerateAuditIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync<TId>("AuditId", cancellationToken);


        /// <summary>
        /// 生成迁移标识。
        /// </summary>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public virtual TId GenerateMigrationId()
            => GenerateId<TId>("MigrationId");

        /// <summary>
        /// 异步生成迁移标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public virtual Task<TId> GenerateMigrationIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync<TId>("MigrationId", cancellationToken);


        /// <summary>
        /// 生成表格标识。
        /// </summary>
        /// <returns>返回 <typeparamref name="TId"/>。</returns>
        public virtual TId GenerateTabulationId()
            => GenerateId<TId>("TabulationId");

        /// <summary>
        /// 异步生成表格标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public virtual Task<TId> GenerateTabulationIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync<TId>("TabulationId", cancellationToken);


        /// <summary>
        /// 生成租户标识。
        /// </summary>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public virtual TId GenerateTenantId()
            => GenerateId<TId>("TenantId");

        /// <summary>
        /// 异步生成租户标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public virtual Task<TId> GenerateTenantIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync<TId>("TenantId", cancellationToken);
    }
}
