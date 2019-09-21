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
        public NetworkBuilder(IExtensionBuilder builder, NetworkBuilderDependencyOptions dependencyOptions)
            : base(builder, dependencyOptions)
        {
            Services.AddSingleton<INetworkBuilder>(this);
        }


        public IExtensionBuilderDependencyOptions DotNettyDependencyOptions { get; private set; }


        public INetworkBuilder AddDotNettyDependencyOptions(IExtensionBuilderDependencyOptions dependencyOptions)
        {
            DotNettyDependencyOptions = dependencyOptions.NotNull(nameof(dependencyOptions));
            return this;
        }
    }
}
