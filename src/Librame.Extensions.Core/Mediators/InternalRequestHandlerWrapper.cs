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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 内部请求处理程序包装。
    /// </summary>
    /// <typeparam name="TRequest">指定的请求类型。</typeparam>
    /// <typeparam name="TResponse">指定的响应类型。</typeparam>
    internal class InternalRequestHandlerWrapper<TRequest, TResponse> : AbstractRequestHandlerWrapper<TResponse>
        where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// 异步处理。
        /// </summary>
        /// <param name="request">给定的 <see cref="IRequest{TResponse}"/>。</param>
        /// <param name="serviceFactory">给定的 <see cref="ServiceFactoryDelegate"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TResponse"/> 异步操作。</returns>
        public override Task<TResponse> HandleAsync(IRequest<TResponse> request, ServiceFactoryDelegate serviceFactory,
            CancellationToken cancellationToken = default)
        {
            Task<TResponse> Handler() => serviceFactory.InvokeHandler<IRequestHandler<TRequest, TResponse>>()
                .HandleAsync((TRequest)request, cancellationToken);

            return serviceFactory
                .Invokes<IPipelineBehavior<TRequest, TResponse>>()
                .Reverse()
                .Aggregate((RequestHandlerDelegate<TResponse>)Handler,
                    (next, pipeline) => () => pipeline.HandleAsync((TRequest)request, next, cancellationToken))();
        }

    }
}
