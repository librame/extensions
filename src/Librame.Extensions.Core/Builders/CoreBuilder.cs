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

namespace Librame.Extensions.Core.Builders
{
    internal class CoreBuilder : AbstractExtensionBuilder, ICoreBuilder
    {
        public CoreBuilder(IServiceCollection services, CoreBuilderDependency dependencyOptions)
            : base(services, dependencyOptions)
        {
            Services.AddSingleton<ICoreBuilder>(this);
            Services.AddSingleton(sp => (IExtensionBuilder)sp.GetRequiredService<ICoreBuilder>());
        }
    }
}
