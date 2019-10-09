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
    static class MediatorDataBuilderExtensions
    {
        public static IDataBuilder AddMediators(this IDataBuilder builder)
        {
            builder.Services.AddTransient(typeof(AuditNotificationHandler<,>));
            builder.Services.AddTransient(typeof(EntityNotificationHandler<,>));
            builder.Services.AddTransient(typeof(MigrationNotificationHandler<,>));

            return builder;
        }

    }
}
