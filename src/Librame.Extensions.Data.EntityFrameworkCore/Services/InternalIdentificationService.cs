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
    /// 内部标识服务。
    /// </summary>
    public class InternalIdentificationService : AbstractService<InternalIdentificationService>, IIdentificationService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalIdentificationService"/> 实例。
        /// </summary>
        /// <param name="logger">给定的 <see cref="ILogger{IdService}"/>。</param>
        public InternalIdentificationService(ILogger<InternalIdentificationService> logger)
            : base(logger)
        {
        }


        /// <summary>
        /// 获取标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="parameters">给定的参数数组。</param>
        /// <returns>返回 <see cref="string"/>。</returns>
        public Task<string> GetIdAsync(CancellationToken cancellationToken, params object[] parameters)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var id = UniqueIdentifier.NewByGuid().ToString();
            Logger.LogInformation($"Get Id: {id}");

            return Task.FromResult(id);
        }
    }
}
