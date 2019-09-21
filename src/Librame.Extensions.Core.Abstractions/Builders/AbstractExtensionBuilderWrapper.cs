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
    /// 抽象扩展构建器封装器。
    /// </summary>
    /// <typeparam name="TBuilder">指定的构建器类型。</typeparam>
    public abstract class AbstractExtensionBuilderWrapper<TBuilder> : AbstractExtensionBuilder, IExtensionBuilderWrapper<TBuilder>
        where TBuilder : class
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilder"/>。
        /// </summary>
        /// <param name="rawBuilder">给定的原始 <typeparamref name="TBuilder"/>。</param>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="dependencyOptions">给定的 <see cref="IExtensionBuilderDependencyOptions"/>。</param>
        protected AbstractExtensionBuilderWrapper(TBuilder rawBuilder,
            IServiceCollection services, IExtensionBuilderDependencyOptions dependencyOptions)
            : base(services, dependencyOptions)
        {
            RawBuilder = rawBuilder.NotNull(nameof(rawBuilder));
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilder"/>。
        /// </summary>
        /// <param name="rawBuilder">给定的原始 <typeparamref name="TBuilder"/>。</param>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependencyOptions">给定的 <see cref="IExtensionBuilderDependencyOptions"/>。</param>
        protected AbstractExtensionBuilderWrapper(TBuilder rawBuilder,
            IExtensionBuilder builder, IExtensionBuilderDependencyOptions dependencyOptions)
            : base(builder, dependencyOptions)
        {
            RawBuilder = rawBuilder.NotNull(nameof(rawBuilder));
        }


        /// <summary>
        /// 原始构建器。
        /// </summary>
        public TBuilder RawBuilder { get; }
    }
}
