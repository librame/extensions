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

namespace Librame.Extensions.Storage
{
    using Core;

    /// <summary>
    /// 存储令牌服务接口。
    /// </summary>
    public interface IStorageTokenService : IService
    {
        /// <summary>
        /// 异步获取令牌。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        Task<string> GetTokenAsync(CancellationToken cancellationToken = default);
    }
}
