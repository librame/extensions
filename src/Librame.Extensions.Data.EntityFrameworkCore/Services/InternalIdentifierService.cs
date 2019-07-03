﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 内部标识符服务。
    /// </summary>
    internal class InternalIdentifierService : AbstractService, IIdentifierService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalIdentifierService"/> 实例。
        /// </summary>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public InternalIdentifierService(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        /// <summary>
        /// 异步获取审计标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="string"/>。</returns>
        public Task<string> GetAuditIdAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                string auditId = GuIdentifier.New();
                Logger.LogInformation($"Get AuditId: {auditId}");

                return auditId;
            });
        }

        /// <summary>
        /// 异步获取租户标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="string"/>。</returns>
        public Task<string> GetTenantIdAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                string tenantId = GuIdentifier.New();
                Logger.LogInformation($"Get TenantId: {tenantId}");

                return tenantId;
            });
        }

    }
}
