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
    /// 内部请求程序网络构建器静态扩展。
    /// </summary>
    internal static class InternalRequesterNetworkBuilderExtensions
    {
        /// <summary>
        /// 添加请求程序集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="INetworkBuilder"/>。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddRequesters(this INetworkBuilder builder)
        {
            builder.Services.AddSingleton<IUriRequester, InternalHttpClientRequester>();
            builder.Services.AddSingleton<IUriRequester, InternalHttpWebRequester>();

            return builder;
        }

    }
}
