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
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 内部中介者。
    /// </summary>
    internal class InternalMediator : IMediator
    {
        private static readonly ConcurrentDictionary<Type, IRequestHandlerWrapper> _requestHandlers
            = new ConcurrentDictionary<Type, IRequestHandlerWrapper>();

        private static readonly ConcurrentDictionary<Type, INotificationHandlerWrapper> _notificationHandlers
            = new ConcurrentDictionary<Type, INotificationHandlerWrapper>();

        private readonly IServiceProvider _serviceProvider = null;


        /// <summary>
        /// 构造一个 <see cref="InternalMediator"/> 实例。
        /// </summary>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        public InternalMediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider.NotNull(nameof(serviceProvider));
        }


        /// <summary>
        /// 异步处理请求。
        /// </summary>
        /// <param name="request">给定的请求。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TResponse"/> 的异步操作。</returns>
        public Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest<TResponse>
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var handlerWrapper = (IRequestHandlerWrapper<TRequest, TResponse>)_requestHandlers.GetOrAdd(typeof(TRequest),
                type => _serviceProvider.GetRequiredService<IRequestHandlerWrapper<TRequest, TResponse>>());

            return handlerWrapper.HandleAsync(request, _serviceProvider, cancellationToken);
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
            var handlerWrapperType = typeof(IRequestHandlerWrapper<,>).MakeGenericType(requestType, typeof(TResponse));

            var handlerWrapper = _requestHandlers.GetOrAdd(requestType,
                type => (IRequestHandlerWrapper)_serviceProvider.GetRequiredService(handlerWrapperType));

            //var method = handlerWrapperType.GetMethod("HandleAsync");
            //return (Task<TResponse>)method.Invoke(handlerWrapper, new object[]
            //{
            //    request, _serviceProvider, cancellationToken
            //});
            
            return ((dynamic)handlerWrapper).HandleAsync(request, _serviceProvider, cancellationToken);
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

            if (notification is INotification _notification)
                return Publish(_notification, cancellationToken);

            throw new ArgumentException($"{nameof(notification)} does not implement ${nameof(INotification)}");
        }

        /// <summary>
        /// 异步向多个处理程序发送通知。
        /// </summary>
        /// <param name="notification">给定的 <see cref="INotification"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            var handlerWrapper = (INotificationHandlerWrapper<TNotification>)_notificationHandlers.GetOrAdd(typeof(TNotification),
                type => _serviceProvider.GetRequiredService<INotificationHandlerWrapper<TNotification>>());

            return handlerWrapper.HandleAsync(notification, _serviceProvider, cancellationToken);
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
            var handlerWrapperType = typeof(INotificationHandlerWrapper<>).MakeGenericType(notificationType);

            var handlerWrapper = _notificationHandlers.GetOrAdd(notificationType,
                type => (INotificationHandlerWrapper)_serviceProvider.GetRequiredService(handlerWrapperType));

            //var method = handlerWrapperType.GetMethod("HandleAsync");
            //return (Task)method.Invoke(handlerWrapper, new object[]
            //{
            //    notification, _serviceProvider, cancellationToken
            //});

            return ((dynamic)handlerWrapper).HandleAsync(notification, _serviceProvider, cancellationToken);
        }

    }
}
