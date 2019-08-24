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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 标识符服务基类。
    /// </summary>
    public class IdentifierServiceBase : AbstractService, IIdentifierService
    {
        /// <summary>
        /// 构造一个 <see cref="IdentifierServiceBase"/>。
        /// </summary>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public IdentifierServiceBase(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        /// <summary>
        /// 异步获取审计标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public virtual Task<string> GetAuditIdAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                string auditId = UniqueIdentifier.New();
                Logger.LogInformation($"Get AuditId: {auditId}");

                return auditId;
            });
        }

        /// <summary>
        /// 异步获取审计属性标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public virtual Task<string> GetAuditPropertyIdAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                string auditPropertyId = UniqueIdentifier.New();
                Logger.LogInformation($"Get AuditPropertyId: {auditPropertyId}");

                return auditPropertyId;
            });
        }


        /// <summary>
        /// 异步获取租户标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public virtual Task<string> GetTenantIdAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                string tenantId = UniqueIdentifier.New();
                Logger.LogInformation($"Get TenantId: {tenantId}");

                return tenantId;
            });
        }

    }
}
