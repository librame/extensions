#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core.Converters
{
    using Transformers;

    /// <summary>
    /// 抽象转换器。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    /// <typeparam name="TTarget">指定的目标类型。</typeparam>
    public abstract class AbstractConverter<TSource, TTarget>
        : AbstractTransformer<TSource, TTarget>, IConverter<TSource, TTarget>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractTransformer{TSource, TTarget}"/>。
        /// </summary>
        /// <param name="forward">给定的正向转换工厂方法。</param>
        /// <param name="reverse">给定的反向转换工厂方法。</param>
        protected AbstractConverter(Func<TSource, TTarget> forward,
            Func<TTarget, TSource> reverse)
            : base(forward, reverse)
        {
        }


        /// <summary>
        /// 来源转自目标。
        /// </summary>
        /// <param name="target">给定的 <typeparamref name="TTarget"/>。</param>
        /// <returns>返回 <typeparamref name="TSource"/>。</returns>
        public virtual TSource ConvertFrom(TTarget target)
            => Reverse.Invoke(target);

        /// <summary>
        /// 来源转为目标。
        /// </summary>
        /// <param name="source">给定的 <typeparamref name="TSource"/>。</param>
        /// <returns>返回 <typeparamref name="TTarget"/>。</returns>
        public virtual TTarget ConvertTo(TSource source)
            => Forward.Invoke(source);
    }
}
