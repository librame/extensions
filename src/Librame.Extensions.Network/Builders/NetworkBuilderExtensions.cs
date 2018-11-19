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
        /// <param name="builderOptions">给定的 <see cref="NetworkBuilderOptions"/>（可选）。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选）。</param>
        /// <param name="postConfigureOptions">给定的 <see cref="Action{NetworkBuilderOptions}"/>（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddNetwork(this IBuilder builder, NetworkBuilderOptions builderOptions = null,
            IConfiguration configuration = null, Action<NetworkBuilderOptions> postConfigureOptions = null)
        {
            return builder.AddNetwork<NetworkBuilderOptions>(builderOptions ?? new NetworkBuilderOptions(),
                configuration, postConfigureOptions);
        }
        /// <summary>
        /// 添加网络扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="builderOptions">给定的构建器选项。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选）。</param>
        /// <param name="postConfigureOptions">给定的 <see cref="Action{TBuilderOptions}"/>（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddNetwork<TBuilderOptions>(this IBuilder builder, TBuilderOptions builderOptions,
            IConfiguration configuration = null, Action<TBuilderOptions> postConfigureOptions = null)
            where TBuilderOptions : NetworkBuilderOptions
        {
            var encryptionBuilder = builder.AddEncryption(builderOptions, configuration, postConfigureOptions);

            return encryptionBuilder.AddBuilder(b =>
            {
                return b.AsNetworkBuilder()
                    .AddCrawlers()
                    .AddSenders();
            },
            builderOptions, configuration, postConfigureOptions);
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

    }
}
