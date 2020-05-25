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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Mediators
{
    /// <summary>
    /// 抽象请求处理程序。
    /// </summary>
    /// <typeparam name="TRequest">指定的请求类型。</typeparam>
    /// <typeparam name="TResponse">指定的响应类型。</typeparam>
    public abstract class AbstractRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractRequestHandler{TRequest, TResponse}"/>。
        /// </summary>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractRequestHandler(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory.NotNull(nameof(loggerFactory));
        }


        /// <summary>
        /// 记录器工厂。
        /// </summary>
        /// <value>返回 <see cref="ILoggerFactory"/>。</value>
        public ILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// 记录器。
        /// </summary>
        /// <value>返回 <see cref="ILogger"/>。</value>
        protected virtual ILogger Logger
            => LoggerFactory.CreateLogger(GetType());


        /// <summary>
        /// 异步处理请求。
        /// </summary>
        /// <param name="request">给定的请求。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TResponse"/> 的异步操作。</returns>
        public abstract Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
    }
}
