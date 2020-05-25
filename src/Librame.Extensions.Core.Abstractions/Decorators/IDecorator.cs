#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Decorators
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
        /// <summary>
        /// 实现实例。
        /// </summary>
        /// <value>返回 <typeparamref name="TImplementation"/>。</value>
        new TImplementation Source { get; }
    }


    /// <summary>
    /// 装饰器接口。
    /// </summary>
    /// <typeparam name="TSource">指定的源类型。</typeparam>
    public interface IDecorator<out TSource>
        where TSource : class
    {
        /// <summary>
        /// 源实例。
        /// </summary>
        /// <value>返回 <typeparamref name="TSource"/>。</value>
        TSource Source { get; }
    }
}
