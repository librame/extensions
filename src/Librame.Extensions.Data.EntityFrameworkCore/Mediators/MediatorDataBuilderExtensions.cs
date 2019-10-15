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

namespace Librame.Extensions.Data
{
    static class MediatorDataBuilderExtensions
    {
        internal static IDataBuilder AddMediators(this IDataBuilder builder)
        {
            builder.Services.TryAddTransient(typeof(AuditNotificationHandler<,>));
            builder.Services.TryAddTransient(typeof(EntityNotificationHandler<,>));
            builder.Services.TryAddTransient(typeof(MigrationNotificationHandler<,>));

            return builder;
        }

    }
}
