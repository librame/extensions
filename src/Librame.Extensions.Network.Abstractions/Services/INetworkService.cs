#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
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
