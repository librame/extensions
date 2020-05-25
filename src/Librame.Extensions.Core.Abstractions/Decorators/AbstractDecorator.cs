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
    /// 抽象装饰器。
    /// </summary>
    /// <typeparam name="TSource">指定的源类型。</typeparam>
    /// <typeparam name="TImplementation">指定的实现类型。</typeparam>
    public abstract class AbstractDecorator<TSource, TImplementation> : AbstractDecorator<TSource>, IDecorator<TSource, TImplementation>
        where TSource : class
        where TImplementation : TSource
    {
        /// <summary>
        /// 构造一个抽象装饰器。
        /// </summary>
        /// <param name="implementation">给定的 <typeparamref name="TImplementation"/>。</param>
        protected AbstractDecorator(TImplementation implementation)
            : base(implementation)
        {
            Source = implementation;
        }


        /// <summary>
        /// 实现实例。
        /// </summary>
        /// <value>返回 <typeparamref name="TImplementation"/>。</value>
        public new TImplementation Source { get; }
    }


    /// <summary>
    /// 抽象装饰器。
    /// </summary>
    /// <typeparam name="TSource">指定的源类型。</typeparam>
    public abstract class AbstractDecorator<TSource> : IDecorator<TSource>
        where TSource : class
    {
        /// <summary>
        /// 构造一个抽象装饰器。
        /// </summary>
        /// <param name="source">给定的 <typeparamref name="TSource"/>。</param>
        protected AbstractDecorator(TSource source)
        {
            Source = source.NotNull(nameof(source));
        }


        /// <summary>
        /// 源实例。
        /// </summary>
        /// <value>返回 <typeparamref name="TSource"/>。</value>
        public TSource Source { get; }
    }
}
