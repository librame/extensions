#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core.Builders
{
    using Core.Services;

    /// <summary>
    /// 抽象扩展构建器适配器。
    /// </summary>
    /// <typeparam name="TAdaptionBuilder">指定的适配构建器类型。</typeparam>
    public abstract class AbstractExtensionBuilderAdapter<TAdaptionBuilder> : AbstractExtensionBuilder, IExtensionBuilderAdapter<TAdaptionBuilder>
        where TAdaptionBuilder : IExtensionBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilderAdapter{TAdaptionBuilder}"/>。
        /// </summary>
        /// <param name="parentBuilder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="adaptionBuilder">给定的 <typeparamref name="TAdaptionBuilder"/>。</param>
        protected AbstractExtensionBuilderAdapter(IExtensionBuilder parentBuilder, TAdaptionBuilder adaptionBuilder)
            : base(parentBuilder, adaptionBuilder?.Dependency)
        {
            AdaptionBuilder = adaptionBuilder;
        }


        /// <summary>
        /// 适配构建器。
        /// </summary>
        public TAdaptionBuilder AdaptionBuilder { get; }


        /// <summary>
        /// 获取指定服务类型的特征。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <returns>默认返回 <see cref="ServiceCharacteristics.Singleton(bool)"/>。</returns>
        public override ServiceCharacteristics GetServiceCharacteristics(Type serviceType)
            => ServiceCharacteristics.Singleton();
        
    }
}
