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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 抽象存储标识符。
    /// </summary>
    public abstract class AbstractStoreIdentifier : IStoreIdentifier
    {
        private readonly ILoggerFactory _loggerFactory;


        /// <summary>
        /// 构造一个 <see cref="AbstractStoreIdentifier"/>。
        /// </summary>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public AbstractStoreIdentifier(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory.NotNull(nameof(loggerFactory));
        }


        /// <summary>
        /// 日志。
        /// </summary>
        protected ILogger Logger
            => _loggerFactory.CreateLogger(GetType());


        /// <summary>
        /// 异步生成有顺序的标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="idTraceName">标识跟踪名称。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        protected virtual Task<string> GenerateCombGuidAsync(CancellationToken cancellationToken, string idTraceName)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                var id = EntityUtility.NewCombGuid();
                Logger.LogTrace($"Generate {idTraceName}: {id}");

                return id;
            });
        }

        /// <summary>
        /// 异步生成有顺序的标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="idTraceName">标识跟踪名称。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        protected virtual Task<string> GenerateCombFileTimeAsync(CancellationToken cancellationToken, string idTraceName)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                var id = DateTimeOffset.Now.AsCombFileTime();
                Logger.LogTrace($"Generate {idTraceName}: {id}");

                return id;
            });
        }


        /// <summary>
        /// 异步获取审计标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public virtual Task<string> GetAuditIdAsync(CancellationToken cancellationToken = default)
            => GenerateCombFileTimeAsync(cancellationToken, "AuditId");

        /// <summary>
        /// 异步获取审计属性标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public virtual Task<string> GetAuditPropertyIdAsync(CancellationToken cancellationToken = default)
            => GenerateCombFileTimeAsync(cancellationToken, "AuditPropertyId");


        /// <summary>
        /// 异步获取实体标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public virtual Task<string> GetEntityIdAsync(CancellationToken cancellationToken = default)
            => GenerateCombGuidAsync(cancellationToken, "EntityId");


        /// <summary>
        /// 异步获取迁移标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public virtual Task<string> GetMigrationIdAsync(CancellationToken cancellationToken = default)
            => GenerateCombGuidAsync(cancellationToken, "MigrationId");


        /// <summary>
        /// 异步获取租户标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public virtual Task<string> GetTenantIdAsync(CancellationToken cancellationToken = default)
            => GenerateCombGuidAsync(cancellationToken, "TenantId");
    }
}
