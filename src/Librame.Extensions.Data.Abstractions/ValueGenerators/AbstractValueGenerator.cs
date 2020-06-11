#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.ValueGenerators
{
    using Core.Services;

    /// <summary>
    /// 抽象值生成器。
    /// </summary>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    public abstract class AbstractValueGenerator<TValue> : AbstractService, IValueGenerator<TValue>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractValueGenerator{TValue}"/>。
        /// </summary>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>（可选）。</param>
        protected AbstractValueGenerator(ILoggerFactory loggerFactory = null)
            : base(loggerFactory)
        {
        }


        /// <summary>
        /// 值类型。
        /// </summary>
        public Type ValueType
            => typeof(TValue);


        /// <summary>
        /// 异步获取值。
        /// </summary>
        /// <typeparam name="TInvoke">指定的调用类型。</typeparam>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TValue"/> 的异步操作。</returns>
        public virtual Task<TValue> GetValueAsync<TInvoke>(CancellationToken cancellationToken = default)
            => GetValueAsync(typeof(TInvoke), cancellationToken);

        /// <summary>
        /// 异步获取值。
        /// </summary>
        /// <param name="invokeType">给定的调用类型。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TValue"/> 的异步操作。</returns>
        public abstract Task<TValue> GetValueAsync(Type invokeType, CancellationToken cancellationToken = default);
    }
}
