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
    /// 抽象扩展构建器装饰器。
    /// </summary>
    /// <typeparam name="TSource">指定的源类型。</typeparam>
    public abstract class AbstractExtensionBuilderDecorator<TSource> : AbstractExtensionBuilder, IExtensionBuilderDecorator<TSource>
        where TSource : class
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilder"/>。
        /// </summary>
        /// <param name="source">给定的 <typeparamref name="TSource"/>。</param>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependencyOptions">给定的 <see cref="IExtensionBuilderDependencyOptions"/>。</param>
        protected AbstractExtensionBuilderDecorator(TSource source,
            IExtensionBuilder builder, IExtensionBuilderDependencyOptions dependencyOptions)
            : base(builder, dependencyOptions)
        {
            Source = source.NotNull(nameof(source));
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilder"/>。
        /// </summary>
        /// <param name="source">给定的 <typeparamref name="TSource"/>。</param>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="dependencyOptions">给定的 <see cref="IExtensionBuilderDependencyOptions"/>。</param>
        protected AbstractExtensionBuilderDecorator(TSource source,
            IServiceCollection services, IExtensionBuilderDependencyOptions dependencyOptions)
            : base(services, dependencyOptions)
        {
            Source = source.NotNull(nameof(source));
        }


        /// <summary>
        /// 源构建器。
        /// </summary>
        public TSource Source { get; }
    }
}
