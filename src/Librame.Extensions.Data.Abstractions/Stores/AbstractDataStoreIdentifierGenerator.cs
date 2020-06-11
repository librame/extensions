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
    /// 抽象数据存储标识符生成器。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    public abstract class AbstractDataStoreIdentifierGenerator<TId> : AbstractStoreIdentifierGenerator<TId>, IDataStoreIdentifierGenerator<TId>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractDataStoreIdentifierGenerator{TId}"/>。
        /// </summary>
        /// <param name="generator">给定的 <see cref="IIdentifierGenerator{TId}"/>。</param>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractDataStoreIdentifierGenerator(IClockService clock, IIdentifierGenerator<TId> generator,
            ILoggerFactory loggerFactory)
            : base(clock, generator, loggerFactory)
        {
        }


        /// <summary>
        /// 异步生成标识。
        /// </summary>
        /// <param name="idName">给定的标识名称。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public override async Task<TId> GenerateIdAsync(string idName,
            CancellationToken cancellationToken = default)
        {
            var id = await Generator.GenerateAsync(Clock, cancellationToken)
                .ConfigureAndResultAsync();
            Logger.LogTrace($"Generate {idName}: {id}");

            return id;
        }


        /// <summary>
        /// 异步生成审计标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public virtual Task<TId> GenerateAuditIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("AuditId", cancellationToken);

        /// <summary>
        /// 异步生成实体标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public virtual Task<TId> GenerateEntityIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("EntityId", cancellationToken);

        /// <summary>
        /// 异步生成迁移标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public virtual Task<TId> GenerateMigrationIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("MigrationId", cancellationToken);

        /// <summary>
        /// 异步生成租户标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public virtual Task<TId> GenerateTenantIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("TenantId", cancellationToken);
    }
}
