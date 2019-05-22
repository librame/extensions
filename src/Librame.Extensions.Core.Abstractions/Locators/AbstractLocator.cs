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
    /// 抽象定位器。
    /// </summary>
    /// <typeparam name="TSource">指定的源类型。</typeparam>
    public abstract class AbstractLocator<TSource> : ILocator<TSource>
        where TSource : class
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractLocator{TSource}"/> 实例。
        /// </summary>
        /// <param name="source">给定的定位源。</param>
        public AbstractLocator(TSource source)
        {
            Source = source.NotNull(nameof(source));
        }


        /// <summary>
        /// 定位源。
        /// </summary>
        public TSource Source { get; protected set; }
    }
}
