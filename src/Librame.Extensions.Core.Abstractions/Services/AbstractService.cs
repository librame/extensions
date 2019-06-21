#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象服务。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    public abstract class AbstractService<TService> : AbstractDisposable<TService>, IService
        where TService : class, IService
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractService{TService}"/> 实例。
        /// </summary>
        /// <param name="logger">给定的 <see cref="ILogger{TService}"/>。</param>
        protected AbstractService(ILogger<TService> logger)
        {
            Logger = logger.NotNull(nameof(logger));
        }


        /// <summary>
        /// 记录器。
        /// </summary>
        /// <value>返回 <see cref="ILogger"/>。</value>
        protected ILogger Logger { get; }
    }
}
