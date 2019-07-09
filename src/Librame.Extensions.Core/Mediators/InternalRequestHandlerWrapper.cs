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
    /// 内部请求处理程序封装。
    /// </summary>
    /// <typeparam name="TRequest">指定的请求类型。</typeparam>
    /// <typeparam name="TResponse">指定的响应类型。</typeparam>
    internal class InternalRequestHandlerWrapper<TRequest, TResponse> : IRequestHandlerWrapper<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// 异步处理请求。
        /// </summary>
        /// <param name="request">给定的 <see cref="IRequest{TResponse}"/>。</param>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TResponse"/> 异步操作。</returns>
        public Task<TResponse> HandleAsync(IRequest<TResponse> request, IServiceProvider serviceProvider,
            CancellationToken cancellationToken = default)
        {
            serviceProvider.NotNull(nameof(serviceProvider));

            var behaviors = serviceProvider.GetRequiredService<IEnumerable<IRequestPipelineBehavior<TRequest, TResponse>>>();

            return behaviors
                .Reverse()
                .Aggregate((RequestHandlerDelegate<TResponse>)HandlerCoreAsync,
                    (next, pipeline) => () => pipeline.HandleAsync((TRequest)request, next, cancellationToken))();

            Task<TResponse> HandlerCoreAsync()
            {
                var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

                return handler.HandleAsync((TRequest)request, cancellationToken);
            }
        }

    }
}
