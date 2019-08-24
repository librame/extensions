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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    class NotificationHandlerWrapper<TNotification> : INotificationHandlerWrapper<TNotification>
        where TNotification : INotification
    {
        public Task HandleAsync(INotification notification, IServiceProvider serviceProvider,
            CancellationToken cancellationToken = default)
        {
            serviceProvider.NotNull(nameof(serviceProvider));

            var handlers = serviceProvider.GetRequiredService<IEnumerable<INotificationHandler<TNotification>>>();

            var factories = handlers.Select(handler => new Func<Task>(() => handler.HandleAsync((TNotification)notification, cancellationToken)));

            return PublishAsync();

            async Task PublishAsync()
            {
                foreach (var factory in factories)
                    await factory.Invoke().ConfigureAwait(false);
            }
        }

    }
}
