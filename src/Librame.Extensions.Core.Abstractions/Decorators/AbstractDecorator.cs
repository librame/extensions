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
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>（可选）。</param>
        protected AbstractDecorator(TImplementation implementation, ILoggerFactory loggerFactory = null)
            : base(implementation, loggerFactory)
        {
        }
    }


    /// <summary>
    /// 抽象装饰器。
    /// </summary>
    /// <typeparam name="TSource">指定的源类型。</typeparam>
    public abstract class AbstractDecorator<TSource> : AbstractDisposable, IDecorator<TSource>
        where TSource : class
    {
        /// <summary>
        /// 构造一个抽象装饰器。
        /// </summary>
        /// <param name="source">给定的 <typeparamref name="TSource"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>（可选）。</param>
        protected AbstractDecorator(TSource source, ILoggerFactory loggerFactory = null)
        {
            Source = source.NotNull(nameof(source));
            LoggerFactory = loggerFactory;
        }


        /// <summary>
        /// 服务实例。
        /// </summary>
        /// <value>返回 <typeparamref name="TSource"/>。</value>
        public TSource Source { get; }

        /// <summary>
        /// 日志工厂。
        /// </summary>
        /// <value>返回 <see cref="ILoggerFactory"/>。</value>
        public ILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// 日志。
        /// </summary>
        /// <value>返回 <see cref="ILogger"/>。</value>
        protected virtual ILogger Logger
            => LoggerFactory?.CreateLogger(GetType());


        /// <summary>
        /// 释放服务装饰器。
        /// </summary>
        protected override void DisposeCore()
        {
            if (Source is IDisposable disposable)
            {
                disposable?.Dispose();
                Logger?.LogTrace($"The {GetType().GetSimpleFullName()} was disposed.");
            }
        }
    }
}
