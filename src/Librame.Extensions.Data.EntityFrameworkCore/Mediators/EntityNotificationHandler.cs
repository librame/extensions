#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    class EntityNotificationHandler<TEntity, TGenId> : AbstractNotificationHandler<EntityNotification<TEntity, TGenId>>
        where TEntity : DataEntity<TGenId>
        where TGenId : IEquatable<TGenId>
    {
        public EntityNotificationHandler(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        public override Task HandleAsync(EntityNotification<TEntity, TGenId> notification, CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                Logger.LogInformation($"{notification.Adds.Count} Entities have been registed.");

                return Task.CompletedTask;
            });
        }

    }
}
