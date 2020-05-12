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
    /// 排名接口。
    /// </summary>
    /// <typeparam name="TRank">指定的排序类型（兼容整数、单双精度的排序字段）。</typeparam>
    public interface IRank<TRank> : IRank
        where TRank : struct
    {
        /// <summary>
        /// 排名。
        /// </summary>
        TRank Rank { get; set; }


        /// <summary>
        /// 异步获取排名。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TRank"/> 的异步操作。</returns>
        new Task<TRank> GetRankAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步设置排名。
        /// </summary>
        /// <param name="rank">给定的 <typeparamref name="TRank"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task SetRankAsync(TRank rank, CancellationToken cancellationToken = default);
    }


    /// <summary>
    /// 排名接口。
    /// </summary>
    public interface IRank
    {
        /// <summary>
        /// 异步获取排名。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含排名的异步操作。</returns>
        Task<object> GetRankAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步设置排名。
        /// </summary>
        /// <param name="obj">给定的排名对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task SetRankAsync(object obj, CancellationToken cancellationToken = default);
    }
}
