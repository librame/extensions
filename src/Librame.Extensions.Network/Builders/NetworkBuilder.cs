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

    internal class NetworkBuilder : AbstractExtensionBuilder, INetworkBuilder
    {
        public NetworkBuilder(IExtensionBuilder parentBuilder, NetworkBuilderDependency dependency)
            : base(parentBuilder, dependency)
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
