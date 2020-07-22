#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Identifiers
{
    using Services;

    /// <summary>
    /// 抽象标识生成器接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    public abstract class AbstractIdentityGenerator<TId> : IIdentityGenerator<TId>
    {
        /// <summary>
        /// 标识类型。
        /// </summary>
        public virtual Type IdType
            => typeof(TId);


        /// <summary>
        /// 生成标识。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <returns>返回 <typeparamref name="TId"/>。</returns>
        public abstract TId GenerateId(IClockService clock);

        /// <summary>
        /// 异步生成标识。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public abstract Task<TId> GenerateIdAsync(IClockService clock,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// 生成标识对象。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <returns>返回标识符对象。</returns>
        public virtual object GenerateObjectId(IClockService clock)
            => GenerateId(clock);

        /// <summary>
        /// 异步生成标识对象。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识符对象的异步操作。</returns>
        public virtual async Task<object> GenerateObjectIdAsync(IClockService clock,
            CancellationToken cancellationToken = default)
            => await GenerateIdAsync(clock, cancellationToken).ConfigureAwait();
    }
}
