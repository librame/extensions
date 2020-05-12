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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Resources;

    /// <summary>
    /// 抽象实体。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    [NotMapped]
    public abstract class AbstractEntity<TId> : AbstractEntity<TId, float, DataStatus>, IRank<float>, IStatus<DataStatus>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractEntity{TId}"/> 默认实例。
        /// </summary>
        protected AbstractEntity()
        {
            Rank = DataSettings.Preference.DefaultRank;
            Status = DataSettings.Preference.DefaultStatus;
        }

    }


    /// <summary>
    /// 抽象实体。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TRank">指定的排序类型（兼容整数、单双精度的排序字段）。</typeparam>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    [NotMapped]
    public abstract class AbstractEntity<TId, TRank, TStatus> : AbstractIdentifier<TId>, IRank<TRank>, IStatus<TStatus>
        where TId : IEquatable<TId>
        where TRank : struct
        where TStatus : struct
    {
        /// <summary>
        /// 排序。
        /// </summary>
        [Display(Name = nameof(Rank), GroupName = "DataGroup", ResourceType = typeof(AbstractEntityResource))]
        public virtual TRank Rank { get; set; }

        /// <summary>
        /// 状态。
        /// </summary>
        [Display(Name = nameof(Status), GroupName = "DataGroup", ResourceType = typeof(AbstractEntityResource))]
        public virtual TStatus Status { get; set; }


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
        /// 获取状态。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task{TStatus}"/>。</returns>
        public Task<TStatus> GetStatusAsync(CancellationToken cancellationToken = default)
            => cancellationToken.RunFactoryOrCancellationAsync(() => Status);

        Task<object> IStatus.GetStatusAsync(CancellationToken cancellationToken)
            => cancellationToken.RunFactoryOrCancellationAsync(() => (object)Status);


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


        /// <summary>
        /// 设置状态。
        /// </summary>
        /// <param name="status">给定的 <typeparamref name="TStatus"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task SetStatusAsync(TStatus status, CancellationToken cancellationToken = default)
            => cancellationToken.RunActionOrCancellationAsync(() => Status = status);

        /// <summary>
        /// 设置状态。
        /// </summary>
        /// <param name="obj">给定的状态对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task SetStatusAsync(object obj, CancellationToken cancellationToken = default)
        {
            var status = obj.CastTo<object, TStatus>(nameof(obj));

            return cancellationToken.RunActionOrCancellationAsync(() => Status = status);
        }

    }
}
