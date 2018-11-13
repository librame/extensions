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
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Builders
{
    using Extensions.Encryption;
    using Extensions.Network;

    /// <summary>
    /// 网络构建器静态扩展。
    /// </summary>
    public static class NetworkBuilderExtensions
    {

        /// <summary>
        /// 添加网络扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选）。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{NetworkBuilderOptions}"/>（可选）。</param>
        /// <param name="configureEncryption">给定的 <see cref="Action{EncryptionBuilderOptions}"/>（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddNetwork(this IBuilder builder,
            IConfiguration configuration = null, Action<NetworkBuilderOptions> configureOptions = null,
            Action<EncryptionBuilderOptions> configureEncryption = null)
        {
            builder.PreConfigureBuilder(configuration, configureOptions);

            // 引入加密扩展
            var encryptionBuilder = builder.AddEncryption(configureOptions: configureEncryption);
            
            var networkBuilder = encryptionBuilder.AsNetworkBuilder();

            networkBuilder.AddCrawlers()
                .AddSenders();

            return networkBuilder;
        }


        /// <summary>
        /// 转换为内部网络构建器。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AsNetworkBuilder(this IEncryptionBuilder builder)
        {
            return new InternalNetworkBuilder(builder);
        }

        /// <summary>
        /// 添加抓取器。
        /// </summary>
        /// <param name="builder">给定的 <see cref="INetworkBuilder"/>。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddCrawlers(this INetworkBuilder builder)
        {
            builder.Services.AddSingleton<ICrawlerService, InternalCrawlerService>();

            return builder;
        }

        /// <summary>
        /// 添加发送器。
        /// </summary>
        /// <param name="builder">给定的 <see cref="INetworkBuilder"/>。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddSenders(this INetworkBuilder builder)
        {
            builder.Services.AddSingleton<IEmailSender, InternalEmailSender>();
            builder.Services.AddSingleton<ISmsSender, InternalSmsSender>();

            return builder;
        }

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
