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

namespace Librame.Builders
{
    using Extensions;
    
    /// <summary>
    /// 构建器。
    /// </summary>
    public class Builder : IBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="Builder"/> 实例。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        public Builder(IServiceCollection services)
        {
            Services = services.NotDefault(nameof(services));

            Services.AddSingleton<IBuilder>(this);
        }

        /// <summary>
        /// 构造一个 <see cref="Builder"/> 实例。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        protected Builder(IBuilder builder)
        {
            Services = builder.NotDefault(nameof(builder)).Services;
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
