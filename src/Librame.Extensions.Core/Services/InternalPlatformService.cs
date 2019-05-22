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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 内部平台服务。
    /// </summary>
    internal class InternalPlatformService : AbstractService<InternalPlatformService>, IPlatformService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalPlatformService"/> 实例。
        /// </summary>
        /// <param name="logger">给定的 <see cref="ILogger{InternalPlatformService}"/>。</param>
        public InternalPlatformService(ILogger<InternalPlatformService> logger)
            : base(logger)
        {
        }


        /// <summary>
        /// 异步获取环境信息。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="parameters">给定的参数数组。</param>
        /// <returns>返回一个包含 <see cref="IEnvironmentInfo"/> 的异步操作。</returns>
        public Task<IEnvironmentInfo> GetEnvironmentInfoAsync(CancellationToken cancellationToken, params object[] parameters)
        {
            IEnvironmentInfo info = new ApplicationEnvironmentInfo();
            Logger.LogInformation($"Refresh environment info at {DateTimeOffset.Now.ToString()}");

            return Task.FromResult(info);
        }
    }
}
