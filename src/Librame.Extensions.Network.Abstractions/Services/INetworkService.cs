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
    using Encryption;
    using Services;

    /// <summary>
    /// 网络服务接口。
    /// </summary>
    public interface INetworkService : IService
    {
        /// <summary>
        /// 散列算法。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IHashAlgorithmService"/>。
        /// </value>
        IHashAlgorithmService Hash { get; }

        /// <summary>
        /// 构建器选项。
        /// </summary>
        /// <value>
        /// 返回 <see cref="NetworkBuilderOptions"/>。
        /// </value>
        NetworkBuilderOptions Options { get; }

        /// <summary>
        /// 字符编码（默认使用 <see cref="Encoding.UTF8"/>）。
        /// </summary>
        Encoding Encoding { get; set; }
    }
}
