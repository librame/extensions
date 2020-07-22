#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Resources;

    /// <summary>
    /// 抽象访问用户计数。
    /// </summary>
    public abstract class AbstractVisitUserCount : AbstractVisitUserCount<long>
    {
        /// <summary>
        /// 累减计数。
        /// </summary>
        /// <param name="count">给定要累减的计数。</param>
        /// <returns>返回长整数。</returns>
        public override long DegressiveCount(long count)
            => --count;

        /// <summary>
        /// 异步累减计数。
        /// </summary>
        /// <param name="count">给定要累减的计数。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含长整数的异步操作。</returns>
        public override ValueTask<long> DegressiveCountAsync(long count,
            CancellationToken cancellationToken = default)
            => cancellationToken.RunOrCancelValueAsync(() => --count);


        /// <summary>
        /// 累加计数。
        /// </summary>
        /// <param name="count">给定要累加的计数。</param>
        /// <returns>返回长整数。</returns>
        public override long ProgressiveCount(long count)
            => ++count;

        /// <summary>
        /// 异步累加计数。
        /// </summary>
        /// <param name="count">给定要累加的计数。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含长整数的异步操作。</returns>
        public override ValueTask<long> ProgressiveCountAsync(long count,
            CancellationToken cancellationToken = default)
            => cancellationToken.RunOrCancelValueAsync(() => ++count);
    }


    /// <summary>
    /// 抽象访问用户计数。
    /// </summary>
    /// <typeparam name="TValue">指定的数值类型。</typeparam>
    public abstract class AbstractVisitUserCount<TValue> : AbstractUserCount<TValue>, IVisitUserCount<TValue>
        where TValue : struct
    {
        /// <summary>
        /// 访问次数。
        /// </summary>
        [Display(Name = nameof(VisitCount), ResourceType = typeof(AbstractEntityResource))]
        public virtual TValue VisitCount { get; set; }

        /// <summary>
        /// 访问人数。
        /// </summary>
        [Display(Name = nameof(VisitorCount), ResourceType = typeof(AbstractEntityResource))]
        public virtual TValue VisitorCount { get; set; }


        /// <summary>
        /// 累加访问次数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public virtual TValue ProgressiveVisitCount()
            => VisitCount = ProgressiveCount(VisitCount);

        /// <summary>
        /// 异步累加访问次数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> ProgressiveVisitCountAsync(CancellationToken cancellationToken = default)
            => VisitCount = await ProgressiveCountAsync(VisitCount, cancellationToken).ConfigureAwait();


        /// <summary>
        /// 累加访问人数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public virtual TValue ProgressiveVisitorCount()
            => VisitorCount = ProgressiveCount(VisitorCount);

        /// <summary>
        /// 异步累加访问人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> ProgressiveVisitorCountAsync(CancellationToken cancellationToken = default)
            => VisitorCount = await ProgressiveCountAsync(VisitorCount, cancellationToken).ConfigureAwait();

    }
}
