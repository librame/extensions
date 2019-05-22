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

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 标识服务接口。
    /// </summary>
    public interface IIdentificationService : IService
    {
        /// <summary>
        /// 异步获取标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="parameters">给定的参数数组。</param>
        /// <returns>返回一个包含 <see cref="string"/> 的异步操作。</returns>
        Task<string> GetIdAsync(CancellationToken cancellationToken, params object[] parameters);
    }
}
