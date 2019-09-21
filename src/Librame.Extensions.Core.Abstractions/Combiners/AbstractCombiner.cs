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
    /// 抽象组合器。
    /// </summary>
    /// <typeparam name="TSource">指定的源类型。</typeparam>
    public abstract class AbstractCombiner<TSource> : ICombiner
        where TSource : class
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractCombiner{TSource}"/>。
        /// </summary>
        /// <param name="source">给定的组合源。</param>
        public AbstractCombiner(TSource source)
        {
            Source = RawSource = source.NotNull(nameof(source));
        }


        /// <summary>
        /// 原始源实例。
        /// </summary>
        public TSource RawSource { get; }

        /// <summary>
        /// 源实例。
        /// </summary>
        public virtual TSource Source { get; }
    }
}
