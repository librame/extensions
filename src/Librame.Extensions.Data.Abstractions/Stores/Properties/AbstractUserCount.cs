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
    /// 抽象用户计数。
    /// </summary>
    public abstract class AbstractUserCount : AbstractUserCount<long>
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
    /// 抽象用户计数。
    /// </summary>
    /// <typeparam name="TValue">指定的数值类型。</typeparam>
    public abstract class AbstractUserCount<TValue> : AbstractCount<TValue>, IUserCount<TValue>
        where TValue : struct
    {
        /// <summary>
        /// 支持人数。
        /// </summary>
        [Display(Name = nameof(SupporterCount), ResourceType = typeof(AbstractEntityResource))]
        public virtual TValue SupporterCount { get; set; }

        /// <summary>
        /// 反对人数。
        /// </summary>
        [Display(Name = nameof(ObjectorCount), ResourceType = typeof(AbstractEntityResource))]
        public virtual TValue ObjectorCount { get; set; }

        /// <summary>
        /// 收藏人数。
        /// </summary>
        [Display(Name = nameof(FavoriteCount), ResourceType = typeof(AbstractEntityResource))]
        public virtual TValue FavoriteCount { get; set; }

        /// <summary>
        /// 转发次数。
        /// </summary>
        [Display(Name = nameof(RetweetCount), ResourceType = typeof(AbstractEntityResource))]
        public virtual TValue RetweetCount { get; set; }


        #region Degressive

        /// <summary>
        /// 累减支持人数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public virtual TValue DegressiveSupporterCount()
            => SupporterCount = DegressiveCount(SupporterCount);

        /// <summary>
        /// 异步累减支持人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> DegressiveSupporterCountAsync(CancellationToken cancellationToken = default)
            => SupporterCount = await DegressiveCountAsync(SupporterCount, cancellationToken).ConfigureAwait();


        /// <summary>
        /// 累减反对人数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public virtual TValue DegressiveObjectorCount()
            => ObjectorCount = DegressiveCount(ObjectorCount);

        /// <summary>
        /// 异步累减反对人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> DegressiveObjectorCountAsync(CancellationToken cancellationToken = default)
            => ObjectorCount = await DegressiveCountAsync(ObjectorCount, cancellationToken).ConfigureAwait();


        /// <summary>
        /// 累减收藏人数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public virtual TValue DegressiveFavoriteCount()
            => FavoriteCount = DegressiveCount(FavoriteCount);

        /// <summary>
        /// 异步累减收藏人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> DegressiveFavoriteCountAsync(CancellationToken cancellationToken = default)
            => FavoriteCount = await DegressiveCountAsync(FavoriteCount, cancellationToken).ConfigureAwait();


        /// <summary>
        /// 累减转发次数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public virtual TValue DegressiveRetweetCount()
            => RetweetCount = DegressiveCount(RetweetCount);

        /// <summary>
        /// 异步累减转发次数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> DegressiveRetweetCountAsync(CancellationToken cancellationToken = default)
            => RetweetCount = await DegressiveCountAsync(RetweetCount, cancellationToken).ConfigureAwait();

        #endregion


        #region Progressive

        /// <summary>
        /// 累加支持人数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public virtual TValue ProgressiveSupporterCount()
            => SupporterCount = ProgressiveCount(SupporterCount);

        /// <summary>
        /// 异步累加支持人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> ProgressiveSupporterCountAsync(CancellationToken cancellationToken = default)
            => SupporterCount = await ProgressiveCountAsync(SupporterCount, cancellationToken).ConfigureAwait();


        /// <summary>
        /// 累加反对人数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public virtual TValue ProgressiveObjectorCount()
            => ObjectorCount = ProgressiveCount(ObjectorCount);

        /// <summary>
        /// 异步累加反对人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> ProgressiveObjectorCountAsync(CancellationToken cancellationToken = default)
            => ObjectorCount = await ProgressiveCountAsync(ObjectorCount, cancellationToken).ConfigureAwait();


        /// <summary>
        /// 累加收藏人数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public virtual TValue ProgressiveFavoriteCount()
            => FavoriteCount = ProgressiveCount(FavoriteCount);

        /// <summary>
        /// 异步累加收藏人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> ProgressiveFavoriteCountAsync(CancellationToken cancellationToken = default)
            => FavoriteCount = await ProgressiveCountAsync(FavoriteCount, cancellationToken).ConfigureAwait();


        /// <summary>
        /// 累加转发次数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public virtual TValue ProgressiveRetweetCount()
            => RetweetCount = ProgressiveCount(RetweetCount);

        /// <summary>
        /// 异步累加转发次数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> ProgressiveRetweetCountAsync(CancellationToken cancellationToken = default)
            => RetweetCount = await ProgressiveCountAsync(RetweetCount, cancellationToken).ConfigureAwait();

        #endregion

    }
}
