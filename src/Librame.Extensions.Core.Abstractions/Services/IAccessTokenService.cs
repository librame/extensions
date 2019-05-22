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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 访问令牌服务接口。
    /// </summary>
    public interface IAccessTokenService : IService
    {
        /// <summary>
        /// 获取令牌。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="parameters">给定的参数数组。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        Task<string> GetTokenAsync(CancellationToken cancellationToken, params object[] parameters);
    }
}
