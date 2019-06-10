#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Configuration;
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
        /// <param name="configureOptions">给定的 <see cref="Action{NetworkBuilderOptions}"/>（可选；高优先级）。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选；次优先级）。</param>
        /// <param name="configureBinderOptions">给定的配置绑定器选项动作（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddNetwork(this IBuilder builder,
            Action<NetworkBuilderOptions> configureOptions = null,
            IConfiguration configuration = null,
            Action<BinderOptions> configureBinderOptions = null)
        {
            var options = builder.Configure(configureOptions,
                configuration, configureBinderOptions);

            // Check EncryptionBuilder Dependency
            if (!(builder is IEncryptionBuilder))
            {
                builder.AddEncryption(options.ConfigureEncryption)
                    .AddDeveloperGlobalSigningCredentials();
            }

            var networkBuilder = new InternalNetworkBuilder(builder, options);

            return networkBuilder
                .AddServices();
        }

    }
}
