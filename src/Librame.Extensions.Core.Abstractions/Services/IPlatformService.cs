#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 平台服务接口。
    /// </summary>
    public interface IPlatformService : IService
    {
        /// <summary>
        /// 异步获取环境信息。
        /// </summary>
        /// <returns>返回一个包含 <see cref="IEnvironmentInfo"/> 的异步操作。</returns>
        Task<IEnvironmentInfo> GetEnvironmentInfoAsync();
    }
}
