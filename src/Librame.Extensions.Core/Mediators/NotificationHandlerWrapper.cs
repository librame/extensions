#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Mediators
{
    using Services;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class NotificationHandlerWrapper<TNotification> : INotificationHandlerWrapper<TNotification>
        where TNotification : INotificationIndication
    {
        public Task HandleAsync(INotificationIndication notification, ServiceFactory serviceFactory,
            CancellationToken cancellationToken = default)
        {
            var handlers = serviceFactory.GetRequiredService<IEnumerable<INotificationHandler<TNotification>>>();
            var factories = handlers.Select(handler => new Func<Task>(() => handler.HandleAsync((TNotification)notification, cancellationToken)));

            return PublishAsync();

            async Task PublishAsync()
            {
                foreach (var factory in factories)
                    await factory.Invoke().ConfigureAndWaitAsync();
            }
        }

    }
}
