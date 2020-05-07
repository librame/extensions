#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Services
{
    /// <summary>
    /// 环境服务接口。
    /// </summary>
    public interface IEnvironmentService : IService
    {
        /// <summary>
        /// 异步获取环境信息。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IEnvironmentInfo"/> 的异步操作。</returns>
        Task<IEnvironmentInfo> GetEnvironmentInfoAsync(CancellationToken cancellationToken = default);
    }
}
