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
    /// 抽象封装器。
    /// </summary>
    /// <typeparam name="TSource">指定的源类型。</typeparam>
    public abstract class AbstractWrapper<TSource> : IWrapper<TSource>
        where TSource : class
    {
        /// <summary>
        /// 构造一个抽象封装器。
        /// </summary>
        /// <param name="source">给定的 <typeparamref name="TSource"/>。</param>
        protected AbstractWrapper(TSource source)
        {
            Source = source.NotNull(nameof(source));
        }


        /// <summary>
        /// 源实例。
        /// </summary>
        public TSource Source { get; }
    }
}
