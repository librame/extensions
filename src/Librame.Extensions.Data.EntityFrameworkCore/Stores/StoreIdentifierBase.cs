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
    /// 存储标识符基类。
    /// </summary>
    public class StoreIdentifierBase : IStoreIdentifier
    {
        /// <summary>
        /// 构造一个 <see cref="StoreIdentifierBase"/>。
        /// </summary>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public StoreIdentifierBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<StoreIdentifierBase>();
        }


        /// <summary>
        /// 日志。
        /// </summary>
        protected ILogger Logger { get; }


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
                string combGuid = Guid.NewGuid().AsCombGuid().ToString();
                Logger.LogTrace($"Generate {idTraceName}: {combGuid}");

                return combGuid;
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
