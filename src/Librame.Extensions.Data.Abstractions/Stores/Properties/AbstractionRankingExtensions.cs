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
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 排名静态扩展。
    /// </summary>
    public static class AbstractionRankingExtensions
    {
        /// <summary>
        /// 设置排名。
        /// </summary>
        /// <typeparam name="TRank">指定的排序类型（兼容整数、单双精度的排序字段）。</typeparam>
        /// <param name="ranking">给定的 <see cref="IRanking{TRank}"/>。</param>
        /// <param name="newRankFactory">给定的新 <typeparamref name="TRank"/> 工厂方法。</param>
        /// <returns>返回 <typeparamref name="TRank"/>（兼容整数、单双精度的排序字段）。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static TRank SetRank<TRank>(this IRanking<TRank> ranking,
            Func<TRank, TRank> newRankFactory)
            where TRank : struct
        {
            ranking.NotNull(nameof(ranking));
            newRankFactory.NotNull(nameof(newRankFactory));

            return ranking.Rank = newRankFactory.Invoke(ranking.Rank);
        }

        /// <summary>
        /// 异步设置排名。
        /// </summary>
        /// <typeparam name="TRank">指定的排序类型（兼容整数、单双精度的排序字段）。</typeparam>
        /// <param name="ranking">给定的 <see cref="IRanking{TRank}"/>。</param>
        /// <param name="newRankFactory">给定的新 <typeparamref name="TRank"/> 工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TRank"/>（兼容整数、单双精度的排序字段）的异步操作。</returns>
        public static ValueTask<TRank> SetRankAsync<TRank>(this IRanking<TRank> ranking,
            Func<TRank, TRank> newRankFactory, CancellationToken cancellationToken = default)
            where TRank : struct
        {
            ranking.NotNull(nameof(ranking));
            newRankFactory.NotNull(nameof(newRankFactory));

            return cancellationToken.RunOrCancelValueAsync(()
                => ranking.Rank = newRankFactory.Invoke(ranking.Rank));
        }


        /// <summary>
        /// 设置对象排名。
        /// </summary>
        /// <param name="ranking">给定的 <see cref="IObjectRanking"/>。</param>
        /// <param name="newRankFactory">给定的新对象排名工厂方法。</param>
        /// <returns>返回排名（兼容整数、单双精度的排序字段）。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static object SetObjectRank(this IObjectRanking ranking,
            Func<object, object> newRankFactory)
        {
            ranking.NotNull(nameof(ranking));
            newRankFactory.NotNull(nameof(newRankFactory));

            var newRank = ranking.GetObjectRank();
            return ranking.SetObjectRank(newRankFactory.Invoke(newRank));
        }

        /// <summary>
        /// 异步设置对象排名。
        /// </summary>
        /// <param name="ranking">给定的 <see cref="IObjectRanking"/>。</param>
        /// <param name="newRankFactory">给定的新对象排名工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含排名（兼容整数、单双精度的排序字段）的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static async ValueTask<object> SetObjectRankAsync(this IObjectRanking ranking,
            Func<object, object> newRankFactory, CancellationToken cancellationToken = default)
        {
            ranking.NotNull(nameof(ranking));
            newRankFactory.NotNull(nameof(newRankFactory));

            var newRank = await ranking.GetObjectRankAsync(cancellationToken).ConfigureAwait();
            return await ranking.SetObjectRankAsync(newRankFactory.Invoke(newRank), cancellationToken)
                .ConfigureAwait();
        }

    }
}
