#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Mediators
{
    using Core.Mediators;
    using Data.Stores;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class EntityNotificationHandler<TEntity, TGenId> : AbstractNotificationHandler<EntityNotification<TEntity, TGenId>>
        where TEntity : DataEntity<TGenId>
        where TGenId : IEquatable<TGenId>
    {
        public EntityNotificationHandler(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        public override Task HandleAsync(EntityNotification<TEntity, TGenId> notification,
            CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                if (notification.Adds.IsNotEmpty())
                    Logger.LogInformation($"{notification.Adds.Count} entities added.");

                if (notification.Updates.IsNotEmpty())
                    Logger.LogInformation($"{notification.Updates.Count} entities updated.");

                if (notification.Removes.IsNotEmpty())
                    Logger.LogInformation($"{notification.Removes.Count} entities removed.");

                return Task.CompletedTask;
            });
        }

    }
}
