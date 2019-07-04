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
    /// <summary>
    /// 服务网络构建器静态扩展。
    /// </summary>
    public static class ServiceNetworkBuilderExtensions
    {
        /// <summary>
        /// 添加服务集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="INetworkBuilder"/>。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddServices(this INetworkBuilder builder)
        {
            builder.Services.AddScoped<ICrawlerService, InternalCrawlerService>();
            builder.Services.AddScoped<IEmailService, InternalEmailService>();
            builder.Services.AddScoped<ISmsService, InternalSmsService>();

            return builder;
        }

    }
}
