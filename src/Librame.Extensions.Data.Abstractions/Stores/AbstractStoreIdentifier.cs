#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
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
    using Core.Services;

    /// <summary>
    /// 抽象存储标识符。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    public abstract class AbstractStoreIdentifier<TGenId> : AbstractService, IStoreIdentifier<TGenId>
        where TGenId : IEquatable<TGenId>
    {
        /// <summary>
        /// 构造一个抽象存储标识符。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractStoreIdentifier(IClockService clock, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Clock = clock.NotNull(nameof(clock));
        }


        /// <summary>
        /// 时钟。
        /// </summary>
        public IClockService Clock { get; }


        /// <summary>
        /// 异步生成标识。
        /// </summary>
        /// <param name="idTraceName">标识跟踪名称。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TGenId"/> 的异步操作。</returns>
        protected abstract Task<TGenId> GenerateIdAsync(string idTraceName,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// 异步获取审计标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TGenId"/> 的异步操作。</returns>
        public virtual Task<TGenId> GetAuditIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("AuditId", cancellationToken);

        /// <summary>
        /// 异步获取实体标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TGenId"/> 的异步操作。</returns>
        public virtual Task<TGenId> GetEntityIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("EntityId", cancellationToken);

        /// <summary>
        /// 异步获取迁移标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TGenId"/> 的异步操作。</returns>
        public virtual Task<TGenId> GetMigrationIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("MigrationId", cancellationToken);

        /// <summary>
        /// 异步获取租户标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TGenId"/> 的异步操作。</returns>
        public virtual Task<TGenId> GetTenantIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("TenantId", cancellationToken);
    }
}
