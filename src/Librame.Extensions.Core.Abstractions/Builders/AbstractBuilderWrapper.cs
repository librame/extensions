#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象构建器封装器。
    /// </summary>
    /// <typeparam name="TBuilder">指定的构建器类型。</typeparam>
    public abstract class AbstractBuilderWrapper<TBuilder> : IBuilderWrapper<TBuilder>
        where TBuilder : class
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractBuilderWrapper{TBuilder}"/>。
        /// </summary>
        /// <param name="rawBuilder">给定的原始 <typeparamref name="TBuilder"/>。</param>
        protected AbstractBuilderWrapper(TBuilder rawBuilder)
        {
            RawBuilder = rawBuilder.NotNull(nameof(rawBuilder));
        }


        /// <summary>
        /// 原始构建器。
        /// </summary>
        public TBuilder RawBuilder { get; }
    }
}
