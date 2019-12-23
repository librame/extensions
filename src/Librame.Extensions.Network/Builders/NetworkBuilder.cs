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
    using Core.Builders;

    class NetworkBuilder : AbstractExtensionBuilder, INetworkBuilder
    {
        public NetworkBuilder(IExtensionBuilder builder, NetworkBuilderDependency dependency)
            : base(builder, dependency)
        {
            Services.AddSingleton<INetworkBuilder>(this);
        }


        public IExtensionBuilderDependency DotNettyDependency { get; private set; }


        public INetworkBuilder AddDotNettyDependency(IExtensionBuilderDependency dependency)
        {
            DotNettyDependency = dependency.NotNull(nameof(dependency));
            return this;
        }
    }
}
