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
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;

namespace Librame.Extensions.Data.Builders
{
    using Aspects;

    static class AspectDataBuilderExtensions
    {
        internal static IDataBuilder AddAspects(this IDataBuilder builder)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(ISaveChangesDbContextAccessorAspect<,,,,,,>),
                typeof(DataAuditSaveChangesDbContextAccessorAspect<,,,,,,>)));

            builder.Services.TryAddEnumerable(new List<ServiceDescriptor>
            {
                ServiceDescriptor.Singleton(typeof(IMigrateDbContextAccessorAspect<,,,,,,>),
                    typeof(DataEntityMigrateDbContextAccessorAspect<,,,,,,>)),
                ServiceDescriptor.Singleton(typeof(IMigrateDbContextAccessorAspect<,,,,,,>),
                    typeof(DataMigrationMigrateDbContextAccessorAspect<,,,,,,>))
            });

            return builder;
        }

    }
}
