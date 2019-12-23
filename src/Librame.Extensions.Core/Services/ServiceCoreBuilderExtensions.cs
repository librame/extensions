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
            builder.Services.TryAddScoped(typeof(IServicesManager<>), typeof(ServicesManager<>));

            builder.Services.TryAddScoped<IClockService, ClockService>();
            builder.Services.TryAddScoped<IHumanizationService, HumanizationService>();
            builder.Services.TryAddScoped<IInjectionService, InjectionService>();
            builder.Services.TryAddScoped<IEnvironmentService, EnvironmentService>();

            return builder;
        }

    }
}
