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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    class RequestHandlerWrapper<TRequest, TResponse> : IRequestHandlerWrapper<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public Task<TResponse> HandleAsync(IRequest<TResponse> request, ServiceFactoryDelegate serviceFactory,
            CancellationToken cancellationToken = default)
        {
            var behaviors = serviceFactory.GetRequiredService<IEnumerable<IRequestPipelineBehavior<TRequest, TResponse>>>();

            return behaviors
                .Reverse()
                .Aggregate((RequestHandlerDelegate<TResponse>)HandlerCoreAsync,
                    (next, pipeline) => () => pipeline.HandleAsync((TRequest)request, next, cancellationToken))();

            Task<TResponse> HandlerCoreAsync()
            {
                var handler = serviceFactory.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

                return handler.HandleAsync((TRequest)request, cancellationToken);
            }
        }

    }
}
