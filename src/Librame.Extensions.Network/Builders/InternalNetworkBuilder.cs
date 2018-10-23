#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;

namespace Librame.Extensions.Network
{
    using Builders;
    using Encryption;

    /// <summary>
    /// 内部网络构建器。
    /// </summary>
    internal class InternalNetworkBuilder : DefaultBuilder, INetworkBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="InternalNetworkBuilder"/> 实例。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        public InternalNetworkBuilder(IEncryptionBuilder builder)
            : base(builder)
        {
            Cryptography = builder;

            Services.AddSingleton<INetworkBuilder>(this);
        }


        /// <summary>
        /// 密码构建器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IEncryptionBuilder"/>。
        /// </value>
        public IEncryptionBuilder Cryptography { get; private set; }

    }
}
