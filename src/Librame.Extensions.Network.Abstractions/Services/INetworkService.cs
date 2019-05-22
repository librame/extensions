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

namespace Librame.Extensions.Network
{
    using Core;
    using Encryption;

    /// <summary>
    /// 网络服务接口。
    /// </summary>
    public interface INetworkService : IService
    {
        /// <summary>
        /// 散列算法。
        /// </summary>
        IHashService Hash { get; }

        /// <summary>
        /// 字符编码（默认使用 <see cref="Encoding.UTF8"/>）。
        /// </summary>
        Encoding Encoding { get; set; }
    }
}
