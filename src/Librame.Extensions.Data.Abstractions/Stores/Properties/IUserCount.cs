#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 用户计数接口。
    /// </summary>
    /// <typeparam name="TValue">指定的数值类型。</typeparam>
    public interface IUserCount<TValue> : ICount<TValue>
        where TValue : struct
    {
        /// <summary>
        /// 支持人数。
        /// </summary>
        /// <value>返回 <typeparamref name="TValue"/>。</value>
        TValue SupporterCount { get; set; }

        /// <summary>
        /// 反对人数。
        /// </summary>
        /// <value>返回 <typeparamref name="TValue"/>。</value>
        TValue ObjectorCount { get; set; }

        /// <summary>
        /// 收藏人数。
        /// </summary>
        /// <value>返回 <typeparamref name="TValue"/>。</value>
        TValue FavoriteCount { get; set; }

        /// <summary>
        /// 转发次数。
        /// </summary>
        TValue RetweetCount { get; set; }


        /// <summary>
        /// 异步累加顶数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> ProgressiveSupporterCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步累加踩数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> ProgressiveObjectorCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步累加收藏数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> ProgressiveFavoriteCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步累加转发次数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> ProgressiveRetweetCountAsync(CancellationToken cancellationToken = default);
    }
}
