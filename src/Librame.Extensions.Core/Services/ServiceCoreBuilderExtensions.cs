#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Librame.Extensions.Core.Builders
{
    using Services;

    static class ServiceCoreBuilderExtensions
    {
        internal static ICoreBuilder AddServices(this ICoreBuilder builder)
        {
            builder.Services.TryAddTransient<ServiceFactory>(serviceProvider => serviceProvider.GetService);

            builder.Services.TryAddTransient(typeof(IServicesManager<,>), typeof(ServicesManager<,>));
            builder.Services.TryAddTransient(typeof(IServicesManager<>), typeof(ServicesManager<>));

            builder.Services.TryAddTransient<IHumanizationService, HumanizationService>();
            builder.Services.TryAddTransient<IInjectionService, InjectionService>();

            builder.Services.TryAddSingleton<IClockService, ClockService>();
            builder.Services.TryAddSingleton<IEnvironmentService, EnvironmentService>();

            return builder;
        }

    }
}
