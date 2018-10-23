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
    using Extensions;
    using Extensions.Encryption;
    using Extensions.Network;

    /// <summary>
    /// 网络构建器静态扩展。
    /// </summary>
    public static class NetworkBuilderExtensions
    {

        /// <summary>
        /// 添加网络。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{INetworkBuilderOptions}"/>。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddNetwork(this IBuilder builder,
            IConfiguration configuration = null, Action<INetworkBuilderOptions> configureOptions = null)
        {
            return builder.AddNetwork<DefaultNetworkBuilderOptions>(configuration, configureOptions);
        }
        /// <summary>
        /// 添加网络。
        /// </summary>
        /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{TBuilderOptions}"/>。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddNetwork<TBuilderOptions>(this IBuilder builder,
            IConfiguration configuration = null, Action<TBuilderOptions> configureOptions = null)
            where TBuilderOptions : class, INetworkBuilderOptions
        {
            if (configuration.IsNotDefault())
                builder.Services.Configure<TBuilderOptions>(configuration);
            
            if (configureOptions.IsNotDefault())
                builder.Services.Configure(configureOptions);
            
            // 引用密码扩展
            var cryptographyBuilder = builder.AddEncryption(configureOptions: configureOptions);
            
            var networkBuilder = cryptographyBuilder.AsNetworkBuilder();

            networkBuilder.AddCrawler()
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
        public static INetworkBuilder AddCrawler(this INetworkBuilder builder)
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

    }
}
