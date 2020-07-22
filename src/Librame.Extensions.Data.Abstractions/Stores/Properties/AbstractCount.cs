#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 抽象计数。
    /// </summary>
    /// <typeparam name="TValue">指定的数值类型。</typeparam>
    public abstract class AbstractCount<TValue> : ICount<TValue>
        where TValue : struct
    {
        /// <summary>
        /// 值类型。
        /// </summary>
        [NotMapped]
        public virtual Type ValueType
            => typeof(TValue);


        /// <summary>
        /// 累减对象计数。
        /// </summary>
        /// <param name="count">给定要累减的计数对象。</param>
        /// <returns>返回对象。</returns>
        public virtual object DegressiveObjectCount(object count)
        {
            var realCount = count.CastTo<object, TValue>(nameof(count));
            DegressiveCount(realCount);

            return count;
        }

        /// <summary>
        /// 异步累减对象计数。
        /// </summary>
        /// <param name="count">给定要累减的计数对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{Object}"/>。</returns>
        public virtual async ValueTask<object> DegressiveObjectCountAsync(object count,
            CancellationToken cancellationToken = default)
        {
            var realCount = count.CastTo<object, TValue>(nameof(count));
            await DegressiveCountAsync(realCount, cancellationToken).ConfigureAwait();

            return count;
        }


        /// <summary>
        /// 累加对象计数。
        /// </summary>
        /// <param name="count">给定要累加的对象计数对象。</param>
        /// <returns>返回对象。</returns>
        public virtual object ProgressiveObjectCount(object count)
        {
            var realCount = count.CastTo<object, TValue>(nameof(count));
            ProgressiveCount(realCount);

            return count;
        }

        /// <summary>
        /// 异步累加对象计数。
        /// </summary>
        /// <param name="count">给定要累加的对象计数对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{Object}"/>。</returns>
        public virtual async ValueTask<object> ProgressiveObjectCountAsync(object count,
            CancellationToken cancellationToken = default)
        {
            var realCount = count.CastTo<object, TValue>(nameof(count));
            await ProgressiveCountAsync(realCount, cancellationToken).ConfigureAwait();

            return count;
        }


        /// <summary>
        /// 累减计数。
        /// </summary>
        /// <param name="count">给定要累减的计数。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public abstract TValue DegressiveCount(TValue count);

        /// <summary>
        /// 异步累减计数。
        /// </summary>
        /// <param name="count">给定要累减的计数。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public abstract ValueTask<TValue> DegressiveCountAsync(TValue count,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// 累加计数。
        /// </summary>
        /// <param name="count">给定要累加的计数。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public abstract TValue ProgressiveCount(TValue count);

        /// <summary>
        /// 异步累加计数。
        /// </summary>
        /// <param name="count">给定要累加的计数。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public abstract ValueTask<TValue> ProgressiveCountAsync(TValue count,
            CancellationToken cancellationToken = default);
    }
}
