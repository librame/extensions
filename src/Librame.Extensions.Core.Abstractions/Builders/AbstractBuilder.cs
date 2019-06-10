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
    /// <typeparam name="TOptions">指定的构建器选项类型。</typeparam>
    public abstract class AbstractBuilder<TOptions> : AbstractBuilder, IBuilder
        where TOptions : class, IBuilderOptions
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractBuilder"/> 实例。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="options">给定的构建器选项实例。</param>
        protected AbstractBuilder(IServiceCollection services, TOptions options)
            : base(services, options)
        {
            Initialize(options);
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractBuilder"/> 实例。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="options">给定的构建器选项实例。</param>
        protected AbstractBuilder(IBuilder builder, TOptions options)
            : base(builder, options)
        {
            Initialize(options);
        }


        /// <summary>
        /// 初始化构建器。
        /// </summary>
        /// <param name="options">给定的构建器选项实例。</param>
        protected virtual void Initialize(TOptions options)
        {
        }
    }


    /// <summary>
    /// 抽象构建器。
    /// </summary>
    public abstract class AbstractBuilder : IBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractBuilder"/> 实例。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="options">给定的 <see cref="IBuilderOptions"/>。</param>
        protected AbstractBuilder(IServiceCollection services, IBuilderOptions options)
        {
            Services = services.NotNull(nameof(services));
            Options = options.NotNull(nameof(options));
            ParentBuilder = null;
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractBuilder"/> 实例。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="options">给定的 <see cref="IBuilderOptions"/>。</param>
        protected AbstractBuilder(IBuilder builder, IBuilderOptions options)
        {
            Services = builder.NotNull(nameof(builder)).Services;
            Options = options.NotNull(nameof(options));
            ParentBuilder = builder;
        }


        /// <summary>
        /// 父构建器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IBuilder"/>。
        /// </value>
        public IBuilder ParentBuilder { get; }

        /// <summary>
        /// 服务集合。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IServiceCollection"/>。
        /// </value>
        public IServiceCollection Services { get; }

        /// <summary>
        /// 构建器选项。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IBuilderOptions"/>。
        /// </value>
        public IBuilderOptions Options { get; }
    }
}
