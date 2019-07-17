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
    /// 抽象扩展构建器。
    /// </summary>
    public abstract class AbstractExtensionBuilder : IExtensionBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilder"/> 实例。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        protected AbstractExtensionBuilder(IServiceCollection services)
        {
            Services = services.NotNull(nameof(services));
            ParentBuilder = null;
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilder"/> 实例。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        protected AbstractExtensionBuilder(IExtensionBuilder builder)
        {
            Services = builder.NotNull(nameof(builder)).Services;
            ParentBuilder = builder;
        }


        /// <summary>
        /// 父构建器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IExtensionBuilder"/>。
        /// </value>
        public IExtensionBuilder ParentBuilder { get; }

        /// <summary>
        /// 服务集合。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IServiceCollection"/>。
        /// </value>
        public IServiceCollection Services { get; }
    }
}
