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


        #region Degressive

        /// <summary>
        /// 累减支持人数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        TValue DegressiveSupporterCount();

        /// <summary>
        /// 异步累减支持人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> DegressiveSupporterCountAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 累减反对人数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        TValue DegressiveObjectorCount();

        /// <summary>
        /// 异步累减反对人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> DegressiveObjectorCountAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 累减收藏人数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        TValue DegressiveFavoriteCount();

        /// <summary>
        /// 异步累减收藏人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> DegressiveFavoriteCountAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 累减转发次数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        TValue DegressiveRetweetCount();

        /// <summary>
        /// 异步累减转发次数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> DegressiveRetweetCountAsync(CancellationToken cancellationToken = default);

        #endregion


        #region Progressive

        /// <summary>
        /// 累加支持人数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        TValue ProgressiveSupporterCount();

        /// <summary>
        /// 异步累加支持人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> ProgressiveSupporterCountAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 累加反对人数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        TValue ProgressiveObjectorCount();

        /// <summary>
        /// 异步累加反对人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> ProgressiveObjectorCountAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 累加收藏人数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        TValue ProgressiveFavoriteCount();

        /// <summary>
        /// 异步累加收藏人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> ProgressiveFavoriteCountAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 累加转发次数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        TValue ProgressiveRetweetCount();

        /// <summary>
        /// 异步累加转发次数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> ProgressiveRetweetCountAsync(CancellationToken cancellationToken = default);

        #endregion

    }
}
