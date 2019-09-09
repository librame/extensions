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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    class EntityNotificationHandler : AbstractNotificationHandler<EntityNotification>
    {
        public EntityNotificationHandler(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        public override Task HandleAsync(EntityNotification notification, CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                Logger.LogInformation($"{notification.Entities.Count} Entities have been registed.");

                return Task.CompletedTask;
            });
        }

    }
}
