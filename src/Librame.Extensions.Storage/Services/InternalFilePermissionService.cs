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
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Storage
{
    using Core;

    /// <summary>
    /// 内部存储令牌服务。
    /// </summary>
    internal class InternalFilePermissionService : StorageServiceBase, IFilePermissionService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalFilePermissionService"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{StorageBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public InternalFilePermissionService(IOptions<StorageBuilderOptions> options,
            ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }


        /// <summary>
        /// 异步获取访问令牌。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public Task<string> GeAccessTokenAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                string accessToken = RandomNumberIdentifier.New();
                Logger.LogInformation($"Get access token: {accessToken}");

                return accessToken;
            });
        }

        /// <summary>
        /// 异步获取授权码。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public Task<string> GetAuthorizationCodeAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                string authorizationCode = RandomNumberIdentifier.New();
                Logger.LogInformation($"Get authorization code: {authorizationCode}");

                return authorizationCode;
            });
        }

        /// <summary>
        /// 异步获取 Cookie 值。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public Task<string> GetCookieValueAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                string cookieValue = RandomNumberIdentifier.New();
                Logger.LogInformation($"Get cookie value: {cookieValue}");

                return cookieValue;
            });
        }
    }
}
