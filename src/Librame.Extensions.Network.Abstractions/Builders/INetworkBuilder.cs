#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network
{
    using Builders;
    using Encryption;

    /// <summary>
    /// 网络构建器接口。
    /// </summary>
    public interface INetworkBuilder : IBuilder
    {
        /// <summary>
        /// 加密构建器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IEncryptionBuilder"/>。
        /// </value>
        IEncryptionBuilder Encryption { get; }
    }
}
