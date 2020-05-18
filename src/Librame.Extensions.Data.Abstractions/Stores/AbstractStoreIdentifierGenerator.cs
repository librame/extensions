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
    using Core.Identifiers;
    using Core.Services;

    /// <summary>
    /// 抽象存储标识符生成器。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    public abstract class AbstractStoreIdentifierGenerator<TGenId> : AbstractStoreIdentifierGenerator, IStoreIdentifierGenerator<TGenId>
        where TGenId : IEquatable<TGenId>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStoreIdentifierGenerator{TGenId}"/>。
        /// </summary>
        /// <param name="generator">给定的 <see cref="IIdentifierGenerator{TIdentifier}"/>。</param>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractStoreIdentifierGenerator(IIdentifierGenerator<TGenId> generator,
            IClockService clock, ILoggerFactory loggerFactory)
            : base(clock, loggerFactory)
        {
            Generator = generator.NotNull(nameof(generator));
        }


        /// <summary>
        /// 标识符生成器。
        /// </summary>
        /// <value>返回 <see cref="IIdentifierGenerator{TIdentifier}"/>。</value>
        protected IIdentifierGenerator<TGenId> Generator { get; }


        /// <summary>
        /// 异步生成标识。
        /// </summary>
        /// <param name="idName">给定的标识名称。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识对象的异步操作。</returns>
        protected override async Task<object> GenerateIdAsync(string idName,
            CancellationToken cancellationToken = default)
        {
            var id = await Generator.GenerateAsync(Clock, cancellationToken)
                .ConfigureAndResultAsync();
            Logger.LogTrace($"Generate {idName}: {id}");

            return id;
        }

        /// <summary>
        /// 异步生成泛型标识。
        /// </summary>
        /// <param name="idName">给定的标识名称。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TGenId"/> 的异步操作。</returns>
        protected virtual async Task<TGenId> GenerateGenericIdAsync(string idName,
            CancellationToken cancellationToken = default)
        {
            var id = await Generator.GenerateAsync(Clock, cancellationToken)
                .ConfigureAndResultAsync();
            Logger.LogTrace($"Generate generic {idName}: {id}");

            return id;
        }


        /// <summary>
        /// 异步生成审计标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TGenId"/> 的异步操作。</returns>
        public new virtual Task<TGenId> GenerateAuditIdAsync(CancellationToken cancellationToken = default)
            => GenerateGenericIdAsync("AuditId", cancellationToken);

        /// <summary>
        /// 异步生成实体标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TGenId"/> 的异步操作。</returns>
        public new virtual Task<TGenId> GenerateEntityIdAsync(CancellationToken cancellationToken = default)
            => GenerateGenericIdAsync("EntityId", cancellationToken);

        /// <summary>
        /// 异步生成迁移标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TGenId"/> 的异步操作。</returns>
        public new virtual Task<TGenId> GenerateMigrationIdAsync(CancellationToken cancellationToken = default)
            => GenerateGenericIdAsync("MigrationId", cancellationToken);

        /// <summary>
        /// 异步生成租户标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TGenId"/> 的异步操作。</returns>
        public new virtual Task<TGenId> GenerateTenantIdAsync(CancellationToken cancellationToken = default)
            => GenerateGenericIdAsync("TenantId", cancellationToken);
    }


    /// <summary>
    /// 抽象存储标识符生成器。
    /// </summary>
    public abstract class AbstractStoreIdentifierGenerator : AbstractService, IStoreIdentifierGenerator
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStoreIdentifierGenerator"/>。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractStoreIdentifierGenerator(IClockService clock, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Clock = clock.NotNull(nameof(clock));
        }


        /// <summary>
        /// 时钟。
        /// </summary>
        /// <value>返回 <see cref="IClockService"/>。</value>
        public IClockService Clock { get; }


        /// <summary>
        /// 异步生成标识。
        /// </summary>
        /// <param name="idName">给定的标识名称。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识对象的异步操作。</returns>
        protected abstract Task<object> GenerateIdAsync(string idName,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// 异步生成审计标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识对象的异步操作。</returns>
        public virtual Task<object> GenerateAuditIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("AuditId", cancellationToken);

        /// <summary>
        /// 异步生成实体标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识对象的异步操作。</returns>
        public virtual Task<object> GenerateEntityIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("EntityId", cancellationToken);

        /// <summary>
        /// 异步生成迁移标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识对象的异步操作。</returns>
        public virtual Task<object> GenerateMigrationIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("MigrationId", cancellationToken);

        /// <summary>
        /// 异步生成租户标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识对象的异步操作。</returns>
        public virtual Task<object> GenerateTenantIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("TenantId", cancellationToken);
    }
}
