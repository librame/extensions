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

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 对象排名接口。
    /// </summary>
    public interface IObjectRanking
    {
        /// <summary>
        /// 排名类型。
        /// </summary>
        Type RankType { get; }


        /// <summary>
        /// 获取对象排名。
        /// </summary>
        /// <returns>返回排名（兼容整数、单双精度的排序字段）。</returns>
        object GetObjectRank();

        /// <summary>
        /// 异步获取对象排名。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含排名（兼容整数、单双精度的排序字段）的异步操作。</returns>
        ValueTask<object> GetObjectRankAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 设置对象排名。
        /// </summary>
        /// <param name="newRank">给定的新排名对象。</param>
        /// <returns>返回排名（兼容整数、单双精度的排序字段）。</returns>
        object SetObjectRank(object newRank);

        /// <summary>
        /// 异步设置对象排名。
        /// </summary>
        /// <param name="newRank">给定的新排名对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含排名（兼容整数、单双精度的排序字段）的异步操作。</returns>
        ValueTask<object> SetObjectRankAsync(object newRank, CancellationToken cancellationToken = default);
    }
}
