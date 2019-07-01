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
    using Core;

    /// <summary>
    /// 内部标识符服务。
    /// </summary>
    internal class InternalIdentifierService : AbstractService<InternalIdentifierService>, IIdentifierService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalIdentifierService"/> 实例。
        /// </summary>
        /// <param name="logger">给定的 <see cref="ILogger{IdService}"/>。</param>
        public InternalIdentifierService(ILogger<InternalIdentifierService> logger)
            : base(logger)
        {
        }


        /// <summary>
        /// 异步获取审计标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="string"/>。</returns>
        public Task<string> GetAuditIdAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string auditId = GuIdentifier.New();
            Logger.LogInformation($"Get AuditId {auditId} in {DateTimeOffset.Now}");

            return Task.FromResult(auditId);
        }

        /// <summary>
        /// 异步获取租户标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="string"/>。</returns>
        public Task<string> GetTenantIdAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string tenantId = GuIdentifier.New();
            Logger.LogInformation($"Get TenantId {tenantId} in {DateTimeOffset.Now}");

            return Task.FromResult(tenantId);
        }

    }
}
