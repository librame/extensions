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
    /// <summary>
    /// 内部通知处理程序封装。
    /// </summary>
    /// <typeparam name="TNotification">指定的通知类型。</typeparam>
    internal class InternalNotificationHandlerWrapper<TNotification> : INotificationHandlerWrapper<TNotification>
        where TNotification : INotification
    {
        /// <summary>
        /// 异步处理通知。
        /// </summary>
        /// <param name="notification">给定的 <see cref="INotification"/>。</param>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
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
