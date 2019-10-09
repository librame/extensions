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
    static class AspectDataBuilderExtensions
    {
        public static IDataBuilder AddAspects(this IDataBuilder builder)
        {
            builder.Services.AddScoped(typeof(ISaveChangesDbContextAccessorAspect<,,,,,,>),
                typeof(DataAuditSaveChangesDbContextAccessorAspect<,,,,,,>));

            builder.Services.AddScoped(typeof(IMigrateDbContextAccessorAspect<,,,,,,>),
                typeof(DataEntityMigrateDbContextAccessorAspect<,,,,,,>));
            builder.Services.AddScoped(typeof(IMigrateDbContextAccessorAspect<,,,,,,>),
                typeof(DataMigrationMigrateDbContextAccessorAspect<,,,,,,>));

            return builder;
        }

    }
}
