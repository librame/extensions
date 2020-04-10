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

namespace Librame.Extensions.Network.Builders
{
    using Requesters;

    static class RequesterNetworkBuilderExtensions
    {
        internal static INetworkBuilder AddRequesters(this INetworkBuilder builder)
        {
            builder.Services.AddSingleton<IUriRequester, HttpClientRequester>();
            builder.Services.AddSingleton<IUriRequester, HttpWebRequester>();

            return builder;
        }

    }
}
