#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Core.Builders
{
    using Core.Services;

    /// <summary>
    /// 抽象扩展构建器装饰器。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    public abstract class AbstractExtensionBuilderDecorator<TSource> : AbstractExtensionBuilder, IExtensionBuilderDecorator<TSource>
        where TSource : class
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilderDecorator{TSource}"/>。
        /// </summary>
        /// <param name="source">给定的装饰 <typeparamref name="TSource"/>。</param>
        /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependency">给定的 <see cref="IExtensionBuilderDependency"/>。</param>
        protected AbstractExtensionBuilderDecorator(TSource source,
            IExtensionBuilder parentBuilder, IExtensionBuilderDependency dependency)
            : base(parentBuilder, dependency)
        {
            Source = source.NotNull(nameof(source));
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilderDecorator{TSource}"/>。
        /// </summary>
        /// <param name="source">给定的装饰 <typeparamref name="TSource"/>。</param>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="dependency">给定的 <see cref="IExtensionBuilderDependency"/>。</param>
        protected AbstractExtensionBuilderDecorator(TSource source,
            IServiceCollection services, IExtensionBuilderDependency dependency)
            : base(services, dependency)
        {
            Source = source.NotNull(nameof(source));
        }


        /// <summary>
        /// 源构建器。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public TSource Source { get; }


        /// <summary>
        /// 获取指定服务类型的特征。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <returns>默认返回 <see cref="ServiceCharacteristics.Singleton(bool)"/>。</returns>
        public override ServiceCharacteristics GetServiceCharacteristics(Type serviceType)
            => ServiceCharacteristics.Singleton();
        
    }
}
