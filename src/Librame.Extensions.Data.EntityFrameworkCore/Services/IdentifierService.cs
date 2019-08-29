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
    /// 标识符服务。
    /// </summary>
    public class IdentifierService : AbstractService, IIdentifierService
    {
        /// <summary>
        /// 构造一个 <see cref="IdentifierService"/>。
        /// </summary>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public IdentifierService(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        /// <summary>
        /// 异步生成标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="idTraceName">标识跟踪名称。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        protected virtual Task<string> GenerateIdAsync(CancellationToken cancellationToken, string idTraceName)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                string combId = UniqueIdentifier.New().ToCombUniqueIdentifier();
                Logger.LogTrace($"Generate {idTraceName}: {combId}");

                return combId;
            });
        }


        /// <summary>
        /// 异步获取审计标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public virtual Task<string> GetAuditIdAsync(CancellationToken cancellationToken = default)
        {
            return GenerateIdAsync(cancellationToken, "AuditId");
        }

        /// <summary>
        /// 异步获取审计属性标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public virtual Task<string> GetAuditPropertyIdAsync(CancellationToken cancellationToken = default)
        {
            return GenerateIdAsync(cancellationToken, "AuditPropertyId");
        }


        /// <summary>
        /// 异步获取租户标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public virtual Task<string> GetTenantIdAsync(CancellationToken cancellationToken = default)
        {
            return GenerateIdAsync(cancellationToken, "TenantId");
        }

    }
}
