#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 内部中介者。
    /// </summary>
    internal class InternalMediator : IMediator
    {
        private static readonly ConcurrentDictionary<Type, AbstractRequestHandlerWrapper> _requestHandlers
            = new ConcurrentDictionary<Type, AbstractRequestHandlerWrapper>();

        private static readonly ConcurrentDictionary<Type, AbstractNotificationHandlerWrapper> _notificationHandlers
            = new ConcurrentDictionary<Type, AbstractNotificationHandlerWrapper>();

        private readonly ServiceFactoryDelegate _serviceFactory = null;


        /// <summary>
        /// 构造一个 <see cref="InternalMediator"/> 实例。
        /// </summary>
        /// <param name="serviceFactory">给定的 <see cref="ServiceFactoryDelegate"/>。</param>
        public InternalMediator(ServiceFactoryDelegate serviceFactory)
        {
            _serviceFactory = serviceFactory.NotNull(nameof(serviceFactory));
        }


        /// <summary>
        /// 异步处理请求。
        /// </summary>
        /// <param name="request">给定的请求。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TResponse"/> 的异步操作。</returns>
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            request.NotNull(nameof(request));

            var requestType = request.GetType();
            var handlerWrapperType = typeof(InternalRequestHandlerWrapper<,>).MakeGenericType(requestType, typeof(TResponse));

            var handlerWrapper = (AbstractRequestHandlerWrapper<TResponse>)_requestHandlers.GetOrAdd(requestType,
                type => (AbstractRequestHandlerWrapper)Activator.CreateInstance(handlerWrapperType));

            return handlerWrapper.HandleAsync(request, _serviceFactory, cancellationToken);
        }


        /// <summary>
        /// 异步向多个处理程序发送通知。
        /// </summary>
        /// <param name="notification">给定的通知对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            notification.NotNull(nameof(notification));

            if (notification is INotification instance)
                return Publish(instance, cancellationToken);

            throw new ArgumentException($"{nameof(notification)} does not implement ${nameof(INotification)}");
        }

        /// <summary>
        /// 异步向多个处理程序发送通知。
        /// </summary>
        /// <param name="notification">给定的 <see cref="INotification"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        public Task Publish(INotification notification, CancellationToken cancellationToken = default)
        {
            var notificationType = notification.GetType();
            var handlerWrapperType = typeof(InternalNotificationHandlerWrapper<>).MakeGenericType(notificationType);

            var handlerWrapper = _notificationHandlers.GetOrAdd(notificationType,
                type => (AbstractNotificationHandlerWrapper)Activator.CreateInstance(handlerWrapperType));

            return handlerWrapper.HandleAsync(notification, _serviceFactory, PublishCore, cancellationToken);
        }

        /// <summary>
        /// 在派生类中重写，以控制如何等待任务。默认情况下，实现是每个处理程序的 foreach 和 wait。
        /// </summary>
        /// <param name="allHandlers">表示调用每个通知处理程序的任务的枚举。</param>
        /// <returns>表示调用所有处理程序的任务。</returns>
        private async Task PublishCore(IEnumerable<Func<Task>> allHandlers)
        {
            foreach (var handler in allHandlers)
            {
                await handler.Invoke().ConfigureAwait(false);
            }
        }

    }
}
