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
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Mediators
{
    using Services;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class ServiceFactoryMediator : IMediator
    {
        private static ConcurrentDictionary<Type, IRequestHandlerWrapperIndication> _requestHandlers
            = new ConcurrentDictionary<Type, IRequestHandlerWrapperIndication>();

        private static ConcurrentDictionary<Type, INotificationHandlerWrapperIndication> _notificationHandlers
            = new ConcurrentDictionary<Type, INotificationHandlerWrapperIndication>();

        private ServiceFactory _serviceFactory = null;
        private ILogger _logger = null;


        public ServiceFactoryMediator(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory.NotNull(nameof(serviceFactory));
            _logger = serviceFactory.GetRequiredService<ILogger<ServiceFactoryMediator>>();
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
                type => (IRequestHandlerWrapperIndication)_serviceFactory.GetRequiredService(handlerWrapperType));

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

            if (notification is INotificationIndication _notification)
                return Publish(_notification, cancellationToken);

            _logger.LogWarning($"{notification.GetType().GetDisplayNameWithNamespace()} does not implement {nameof(INotificationIndication)}");
            return Task.CompletedTask;
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotificationIndication
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            var handlerWrapper = (INotificationHandlerWrapper<TNotification>)_notificationHandlers.GetOrAdd(typeof(TNotification),
                type => _serviceFactory.GetRequiredService<INotificationHandlerWrapper<TNotification>>());

            return handlerWrapper.HandleAsync(notification, _serviceFactory, cancellationToken);
        }

        public Task Publish(INotificationIndication notification, CancellationToken cancellationToken = default)
        {
            var notificationType = notification.GetType();
            var handlerWrapperType = typeof(INotificationHandlerWrapper<>).MakeGenericType(notificationType);

            var handlerWrapper = _notificationHandlers.GetOrAdd(notificationType,
                type => (INotificationHandlerWrapperIndication)_serviceFactory.GetRequiredService(handlerWrapperType));

            //var method = handlerWrapperType.GetMethod("HandleAsync");
            //return (Task)method.Invoke(handlerWrapper, new object[]
            //{
            //    notification, _serviceProvider, cancellationToken
            //});

            return ((dynamic)handlerWrapper).HandleAsync(notification, _serviceFactory, cancellationToken);
        }

    }
}
