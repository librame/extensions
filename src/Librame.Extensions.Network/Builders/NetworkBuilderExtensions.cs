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
        /// <param name="configureOptions">给定的 <see cref="Action{NetworkBuilderOptions}"/>（可选）。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddNetwork(this IBuilder builder,
            Action<NetworkBuilderOptions> configureOptions = null, IConfiguration configuration = null)
        {
            return builder.AddBuilder(configureOptions, configuration, _builder =>
            {
                return _builder.AsNetworkBuilder()
                    .AddCrawlers()
                    .AddSenders();
            });
        }


        /// <summary>
        /// 转换为网络构建器。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AsNetworkBuilder(this IBuilder builder)
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
