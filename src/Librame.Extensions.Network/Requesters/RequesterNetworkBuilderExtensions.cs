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
    static class RequesterNetworkBuilderExtensions
    {
        public static INetworkBuilder AddRequesters(this INetworkBuilder builder)
        {
            builder.Services.AddScoped<IUriRequester, HttpClientRequester>();
            builder.Services.AddScoped<IUriRequester, HttpWebRequester>();

            return builder;
        }

    }
}
