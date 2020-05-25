#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
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
    /// 抽象排名。
    /// </summary>
    /// <typeparam name="TRank">指定的排序类型（兼容整数、单双精度的排序字段）。</typeparam>
    public abstract class AbstractRank<TRank> : IRank<TRank>
        where TRank : struct
    {
        /// <summary>
        /// 排名。
        /// </summary>
        [Display(Name = nameof(Rank), GroupName = "DataGroup", ResourceType = typeof(AbstractEntityResource))]
        public virtual TRank Rank { get; set; }


        /// <summary>
        /// 获取排名。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task{TRank}"/>。</returns>
        public Task<TRank> GetRankAsync(CancellationToken cancellationToken = default)
            => cancellationToken.RunFactoryOrCancellationAsync(() => Rank);

        Task<object> IRank.GetRankAsync(CancellationToken cancellationToken)
            => cancellationToken.RunFactoryOrCancellationAsync(() => (object)Rank);


        /// <summary>
        /// 设置排名。
        /// </summary>
        /// <param name="rank">给定的 <typeparamref name="TRank"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task SetRankAsync(TRank rank, CancellationToken cancellationToken = default)
            => cancellationToken.RunActionOrCancellationAsync(() => Rank = rank);

        /// <summary>
        /// 设置排名。
        /// </summary>
        /// <param name="obj">给定的排名对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task SetRankAsync(object obj, CancellationToken cancellationToken = default)
        {
            var rank = obj.CastTo<object, TRank>(nameof(obj));

            return cancellationToken.RunActionOrCancellationAsync(() => Rank = rank);
        }

    }
}
