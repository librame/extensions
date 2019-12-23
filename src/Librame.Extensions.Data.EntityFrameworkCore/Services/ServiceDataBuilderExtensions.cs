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

namespace Librame.Extensions.Data.Builders
{
    using Services;

    static class ServiceDataBuilderExtensions
    {
        internal static IDataBuilder AddServices(this IDataBuilder builder)
        {
            builder.Services.TryAddScoped(typeof(IMigrationService<,,,,,,>), typeof(MigrationService<,,,,,,>));
            builder.Services.TryAddScoped(typeof(ITenantService<,,,,,,>), typeof(TenantService<,,,,,,>));

            return builder;
        }

    }
}
