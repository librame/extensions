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

namespace Librame.Extensions.Storage
{
    using Core;

    /// <summary>
    /// 内部存储令牌服务。
    /// </summary>
    internal class InternalStorageTokenService : AbstractService<InternalStorageTokenService>, IStorageTokenService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalStorageTokenService"/> 实例。
        /// </summary>
        /// <param name="logger">给定的 <see cref="ILogger{InternalStorageTokenService}"/>。</param>
        public InternalStorageTokenService(ILogger<InternalStorageTokenService> logger)
            : base(logger)
        {
        }


        /// <summary>
        /// 异步令牌。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public Task<string> GetTokenAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string token = RngIdentifier.New();
            Logger.LogInformation($"Get token {token} in {DateTimeOffset.Now}");

            return Task.FromResult(token);
        }
    }
}
