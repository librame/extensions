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
    static class ServiceCoreBuilderExtensions
    {
        public static ICoreBuilder AddServices(this ICoreBuilder builder)
        {
            builder.Services.AddTransient<ServiceFactoryDelegate>(serviceProvider => serviceProvider.GetService);

            builder.Services.AddSingleton(typeof(IServicesManager<>), typeof(ServicesManager<>));
            builder.Services.AddSingleton(typeof(IServicesManager<,>), typeof(ServicesManager<,>));

            builder.Services.AddScoped<IHumanizationService, HumanizationService>();
            builder.Services.AddScoped<IInjectionService, InjectionService>();
            builder.Services.AddScoped<IPlatformService, PlatformService>();

            return builder;
        }

    }
}
