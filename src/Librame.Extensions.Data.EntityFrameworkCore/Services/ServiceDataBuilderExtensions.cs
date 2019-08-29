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

namespace Librame.Extensions.Data
{
    static class ServiceDataBuilderExtensions
    {
        public static IDataBuilder AddServices(this IDataBuilder builder)
        {
            builder.Services.AddScoped<IClockService, ClockService>();
            builder.Services.AddScoped<ITenantService, TenantService>();
            builder.Services.AddScoped<IAuditService, AuditService>();
            builder.Services.AddScoped<IIdentifierService, IdentifierService>();
            builder.Services.AddScoped(typeof(IInitializerService<>), typeof(InitializerService<>));
            builder.Services.AddScoped(typeof(IInitializerService<,>), typeof(InitializerService<,>));

            return builder;
        }

    }
}
