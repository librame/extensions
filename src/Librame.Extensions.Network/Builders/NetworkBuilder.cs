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
    using Core;

    class NetworkBuilder : AbstractExtensionBuilder, INetworkBuilder
    {
        public NetworkBuilder(IExtensionBuilder builder)
            : base(builder)
        {
            Services.AddSingleton<INetworkBuilder>(this);
        }

    }
}
