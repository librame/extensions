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
using System;

namespace Librame.Extensions.Network
{
    using Core;
    using Encryption;

    /// <summary>
    /// 网络构建器静态扩展。
    /// </summary>
    public static class NetworkBuilderExtensions
    {
        /// <summary>
        /// 添加网络扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{NetworkBuilderOptions}"/>（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddNetwork(this IBuilder builder,
            Action<NetworkBuilderOptions> configureOptions = null)
        {
            // Configure Options
            if (configureOptions != null)
                builder.Services.Configure(configureOptions);

            // Check Dependencies
            if (!(builder is IEncryptionBuilder))
                builder.AddEncryption().AddDeveloperGlobalSigningCredentials();

            var networkBuilder = new InternalNetworkBuilder(builder);

            return networkBuilder
                .AddServices();
        }

    }
}
