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

namespace Librame.Extensions.Core
{
    class CoreBuilder : AbstractExtensionBuilder, ICoreBuilder
    {
        public CoreBuilder(IServiceCollection services)
            : base(services)
        {
            Services.AddSingleton<ICoreBuilder>(this);
            Services.AddSingleton(sp => (IExtensionBuilder)sp.GetRequiredService<ICoreBuilder>());
        }
    }
}
