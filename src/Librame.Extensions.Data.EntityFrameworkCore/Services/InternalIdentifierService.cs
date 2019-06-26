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
    public class InternalIdentifierService : AbstractService<InternalIdentifierService>, IIdentifierService
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
        /// 获取标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="string"/>。</returns>
        public Task<string> GetIdAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string id = GuIdentifier.New();
            Logger.LogInformation($"Get Id {id} in {DateTimeOffset.Now}");

            return Task.FromResult(id);
        }
    }
}
