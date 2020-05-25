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
    internal class RequestHandlerWrapper<TRequest, TResponse> : IRequestHandlerWrapper<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public Task<TResponse> HandleAsync(IRequest<TResponse> request, ServiceFactory serviceFactory,
            CancellationToken cancellationToken = default)
        {
            var behaviors = serviceFactory.GetRequiredService<IEnumerable<IRequestPipelineBehavior<TRequest, TResponse>>>();

            return behaviors
                .Reverse()
                .Aggregate((RequestHandler<TResponse>)HandlerCoreAsync,
                    (next, pipeline) => () => pipeline.HandleAsync((TRequest)request, next, cancellationToken))();

            Task<TResponse> HandlerCoreAsync()
            {
                var handler = serviceFactory.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
                return handler.HandleAsync((TRequest)request, cancellationToken);
            }
        }

    }
}
