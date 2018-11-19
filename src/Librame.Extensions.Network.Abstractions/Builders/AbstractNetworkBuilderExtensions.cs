#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Builders
{
    using Extensions.Encryption;
    using Extensions.Network;

    /// <summary>
    /// 抽象网络构建器静态扩展。
    /// </summary>
    public static class AbstractNetworkBuilderExtensions
    {
        
        /// <summary>
        /// 配置加密构建器。
        /// </summary>
        /// <param name="builder">给定的 <see cref="INetworkBuilder"/>。</param>
        /// <param name="configureAction">给定的 <see cref="Action{IEncryptionBuilder}"/>。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder ConfigureEncryption(this INetworkBuilder builder, Action<IEncryptionBuilder> configureAction)
        {
            configureAction?.Invoke(builder.Encryption);

            return builder;
        }

    }
}
