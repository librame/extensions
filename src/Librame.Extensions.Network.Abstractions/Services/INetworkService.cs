#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text;

namespace Librame.Extensions.Network.Services
{
    using Core.Services;

    /// <summary>
    /// 网络服务接口。
    /// </summary>
    public interface INetworkService : IService
    {
        /// <summary>
        /// 字符编码。
        /// </summary>
        Encoding Encoding { get; }
    }
}
