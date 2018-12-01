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
    using Builders;
    using Encryption;
    using Services;

    /// <summary>
    /// 网络服务接口。
    /// </summary>
    public interface INetworkService : IService<NetworkBuilderOptions>
    {
        /// <summary>
        /// 散列算法（默认不使用）。
        /// </summary>
        IHashAlgorithmService Hash { get; set; }

        /// <summary>
        /// 字符编码（默认使用 <see cref="Encoding.UTF8"/>）。
        /// </summary>
        Encoding Encoding { get; set; }
    }
}
