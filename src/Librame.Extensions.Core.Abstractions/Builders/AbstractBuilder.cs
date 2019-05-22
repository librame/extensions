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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象构建器。
    /// </summary>
    public abstract class AbstractBuilder : IBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractBuilder"/> 实例。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        protected AbstractBuilder(IServiceCollection services)
        {
            Services = services.NotNull(nameof(services));
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractBuilder"/> 实例。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        protected AbstractBuilder(IBuilder builder)
        {
            Services = builder.NotNull(nameof(builder)).Services;
        }


        /// <summary>
        /// 服务集合。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IServiceCollection"/>。
        /// </value>
        public IServiceCollection Services { get; }
    }
}
