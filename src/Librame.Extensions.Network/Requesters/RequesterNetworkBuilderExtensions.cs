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
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;

namespace Librame.Extensions.Network.Builders
{
    using Requesters;

    static class RequesterNetworkBuilderExtensions
    {
        internal static INetworkBuilder AddRequesters(this INetworkBuilder builder)
        {
            builder.Services.TryAddEnumerable(new List<ServiceDescriptor>
            {
                ServiceDescriptor.Scoped<IUriRequester, HttpClientRequester>(),
                ServiceDescriptor.Scoped<IUriRequester, HttpWebRequester>()
            });

            return builder;
        }

    }
}
