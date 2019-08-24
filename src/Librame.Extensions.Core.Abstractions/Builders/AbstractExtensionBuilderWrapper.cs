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
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="rawBuilder">给定的原始 <typeparamref name="TBuilder"/>。</param>
        protected AbstractExtensionBuilderWrapper(IServiceCollection services, TBuilder rawBuilder)
            : base(services)
        {
            RawBuilder = rawBuilder.NotNull(nameof(rawBuilder));
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilder"/>。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="rawBuilder">给定的原始 <typeparamref name="TBuilder"/>。</param>
        protected AbstractExtensionBuilderWrapper(IExtensionBuilder builder, TBuilder rawBuilder)
            : base(builder)
        {
            RawBuilder = rawBuilder.NotNull(nameof(rawBuilder));
        }


        /// <summary>
        /// 原始构建器。
        /// </summary>
        public TBuilder RawBuilder { get; }
    }
}
