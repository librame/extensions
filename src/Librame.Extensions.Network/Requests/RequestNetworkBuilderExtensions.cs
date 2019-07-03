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
using System.Net;

namespace Librame.Extensions.Network
{
    /// <summary>
    /// 请求网络构建器静态扩展。
    /// </summary>
    public static class RequestNetworkBuilderExtensions
    {
        /// <summary>
        /// 添加请求集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="INetworkBuilder"/>。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddRequests(this INetworkBuilder builder)
        {
            builder.Services.AddSingleton<IRequestFactory<HttpWebRequest>, InternalHttpWebRequestFactory>();

            return builder;
        }

    }
}
