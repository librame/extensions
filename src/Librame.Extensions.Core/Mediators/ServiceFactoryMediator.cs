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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    class ServiceFactoryMediator : IMediator
    {
        private static readonly ConcurrentDictionary<Type, IRequestHandlerWrapper> _requestHandlers
            = new ConcurrentDictionary<Type, IRequestHandlerWrapper>();

        private static readonly ConcurrentDictionary<Type, INotificationHandlerWrapper> _notificationHandlers
            = new ConcurrentDictionary<Type, INotificationHandlerWrapper>();

        private readonly ServiceFactoryDelegate _serviceFactory = null;


        public ServiceFactoryMediator(ServiceFactoryDelegate serviceFactory)
        {
            _serviceFactory = serviceFactory.NotNull(nameof(serviceFactory));
        }


        public Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest<TResponse>
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var handlerWrapper = (IRequestHandlerWrapper<TRequest, TResponse>)_requestHandlers.GetOrAdd(typeof(TRequest),
                type => _serviceFactory.GetRequiredService<IRequestHandlerWrapper<TRequest, TResponse>>());

            return handlerWrapper.HandleAsync(request, _serviceFactory, cancellationToken);
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            request.NotNull(nameof(request));

            var requestType = request.GetType();
            var handlerWrapperType = typeof(IRequestHandlerWrapper<,>).MakeGenericType(requestType, typeof(TResponse));

            var handlerWrapper = _requestHandlers.GetOrAdd(requestType,
                type => (IRequestHandlerWrapper)_serviceFactory.GetRequiredService(handlerWrapperType));

            //var method = handlerWrapperType.GetMethod("HandleAsync");
            //return (Task<TResponse>)method.Invoke(handlerWrapper, new object[]
            //{
            //    request, _serviceProvider, cancellationToken
            //});
            
            return ((dynamic)handlerWrapper).HandleAsync(request, _serviceFactory, cancellationToken);
        }


        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            notification.NotNull(nameof(notification));

            if (notification is INotification _notification)
                return Publish(_notification, cancellationToken);

            throw new ArgumentException($"{nameof(notification)} does not implement ${nameof(INotification)}");
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            var handlerWrapper = (INotificationHandlerWrapper<TNotification>)_notificationHandlers.GetOrAdd(typeof(TNotification),
                type => _serviceFactory.GetRequiredService<INotificationHandlerWrapper<TNotification>>());

            return handlerWrapper.HandleAsync(notification, _serviceFactory, cancellationToken);
        }

        public Task Publish(INotification notification, CancellationToken cancellationToken = default)
        {
            var notificationType = notification.GetType();
            var handlerWrapperType = typeof(INotificationHandlerWrapper<>).MakeGenericType(notificationType);

            var handlerWrapper = _notificationHandlers.GetOrAdd(notificationType,
                type => (INotificationHandlerWrapper)_serviceFactory.GetRequiredService(handlerWrapperType));

            //var method = handlerWrapperType.GetMethod("HandleAsync");
            //return (Task)method.Invoke(handlerWrapper, new object[]
            //{
            //    notification, _serviceProvider, cancellationToken
            //});

            return ((dynamic)handlerWrapper).HandleAsync(notification, _serviceFactory, cancellationToken);
        }

    }
}
