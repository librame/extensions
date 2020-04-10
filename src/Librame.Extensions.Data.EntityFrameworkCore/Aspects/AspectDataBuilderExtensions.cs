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

namespace Librame.Extensions.Data.Builders
{
    using Aspects;

    static class AspectDataBuilderExtensions
    {
        internal static IDataBuilder AddAspects(this IDataBuilder builder)
        {
            builder.Services.AddSingleton(typeof(DbContextAccessorAspectDependencies<>));

            builder.Services.AddSingleton(typeof(ISaveChangesDbContextAccessorAspect<,,,,,,>),
                typeof(DataAuditSaveChangesDbContextAccessorAspect<,,,,,,>));

            var aspectType = typeof(IMigrateDbContextAccessorAspect<,,,,,,>);
            builder.Services.AddSingleton(aspectType, typeof(DataEntityMigrateDbContextAccessorAspect<,,,,,,>));
            builder.Services.AddSingleton(aspectType, typeof(DataMigrationMigrateDbContextAccessorAspect<,,,,,,>));

            return builder;
        }

    }
}
