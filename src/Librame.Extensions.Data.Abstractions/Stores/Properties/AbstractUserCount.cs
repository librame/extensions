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


        /// <summary>
        /// 异步累加支持人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> ProgressiveSupporterCountAsync(CancellationToken cancellationToken = default)
        {
            SupporterCount = await ProgressiveCountAsync(SupporterCount, cancellationToken).ConfigureAndResultAsync();
            return SupporterCount;
        }

        /// <summary>
        /// 异步累加反对人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> ProgressiveObjectorCountAsync(CancellationToken cancellationToken = default)
        {
            ObjectorCount = await ProgressiveCountAsync(ObjectorCount, cancellationToken).ConfigureAndResultAsync();
            return ObjectorCount;
        }

        /// <summary>
        /// 异步累加收藏人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> ProgressiveFavoriteCountAsync(CancellationToken cancellationToken = default)
        {
            FavoriteCount = await ProgressiveCountAsync(FavoriteCount, cancellationToken).ConfigureAndResultAsync();
            return FavoriteCount;
        }

        /// <summary>
        /// 异步累加转发次数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> ProgressiveRetweetCountAsync(CancellationToken cancellationToken = default)
        {
            RetweetCount = await ProgressiveCountAsync(RetweetCount, cancellationToken).ConfigureAndResultAsync();
            return RetweetCount;
        }

    }
}
