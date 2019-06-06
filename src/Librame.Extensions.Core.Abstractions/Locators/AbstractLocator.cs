﻿#region License

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
            RawSource = source.NotNull(nameof(source));
        }


        /// <summary>
        /// 原始源。
        /// </summary>
        public TSource RawSource { get; }


        /// <summary>
        /// 获取源。
        /// </summary>
        /// <returns>返回源实例。</returns>
        public abstract TSource GetSource();
    }
}