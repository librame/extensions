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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 内部访问令牌服务。
    /// </summary>
    internal class InternalAccessTokenService : AbstractService<InternalAccessTokenService>, IAccessTokenService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalClockService"/> 实例。
        /// </summary>
        /// <param name="logger">给定的 <see cref="ILogger{InternalAccessTokenService}"/>。</param>
        public InternalAccessTokenService(ILogger<InternalAccessTokenService> logger)
            : base(logger)
        {
        }


        /// <summary>
        /// 获取令牌。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="parameters">给定的参数数组。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public Task<string> GetTokenAsync(CancellationToken cancellationToken, params object[] parameters)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var token = UniqueIdentifier.EmptyByGuid.ToString();
            Logger.LogInformation($"Get Access Token: {token}");

            return Task.FromResult(token);
        }
    }
}
