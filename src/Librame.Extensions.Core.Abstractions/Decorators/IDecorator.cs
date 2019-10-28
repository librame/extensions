#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 装饰器接口。
    /// </summary>
    /// <typeparam name="TSource">指定的源类型。</typeparam>
    /// <typeparam name="TImplementation">指定的实现类型。</typeparam>
    public interface IDecorator<out TSource, out TImplementation> : IDecorator<TSource>
        where TSource : class
        where TImplementation : TSource
    {
    }


    /// <summary>
    /// 装饰器接口。
    /// </summary>
    /// <typeparam name="TSource">指定的源类型。</typeparam>
    public interface IDecorator<out TSource> : IDisposable
        where TSource : class
    {
        /// <summary>
        /// 源实例。
        /// </summary>
        /// <value>返回 <typeparamref name="TSource"/>。</value>
        TSource Source { get; }

        /// <summary>
        /// 日志工厂。
        /// </summary>
        /// <value>返回 <see cref="ILoggerFactory"/>。</value>
        ILoggerFactory LoggerFactory { get; }
    }
}
