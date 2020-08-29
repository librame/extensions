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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Core.Identifiers;
    using Data.Resources;

    /// <summary>
    /// 抽象实体（默认已实现 <see cref="IIdentifier{TId}"/>、<see cref="IRanking{Single}"/>、<see cref="IState{DataStatus}"/> 等接口）。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    [NotMapped]
    public abstract class AbstractEntity<TId> : AbstractEntity<TId, float, DataStatus>,
        IRanking<float>, IState<DataStatus>
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
    /// 抽象实体（默认已实现 <see cref="IIdentifier{TId}"/>、<see cref="IRanking{TRank}"/>、<see cref="IState{TStatus}"/> 等接口）。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TRank">指定的排序类型（兼容整数、单双精度的排序字段）。</typeparam>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    [NotMapped]
    public abstract class AbstractEntity<TId, TRank, TStatus> : AbstractIdentifier<TId>,
        IRanking<TRank>, IState<TStatus>
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
        /// 获取排名类型。
        /// </summary>
        [NotMapped]
        public virtual Type RankType
            => typeof(TRank);

        /// <summary>
        /// 获取状态类型。
        /// </summary>
        [NotMapped]
        public virtual Type StatusType
            => typeof(TStatus);


        /// <summary>
        /// 获取对象排名。
        /// </summary>
        /// <returns>返回排名（兼容整数、单双精度的排序字段）。</returns>
        public virtual object GetObjectRank()
            => Rank;

        /// <summary>
        /// 异步获取对象排名。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含排名（兼容整数、单双精度的排序字段）的异步操作。</returns>
        public virtual ValueTask<object> GetObjectRankAsync(CancellationToken cancellationToken)
            => cancellationToken.RunOrCancelValueAsync(() => (object)Rank);


        /// <summary>
        /// 获取对象状态。
        /// </summary>
        /// <returns>返回状态（兼容不支持枚举类型的实体框架）。</returns>
        public virtual object GetObjectStatus()
            => Status;

        /// <summary>
        /// 异步获取对象状态。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含状态（兼容不支持枚举类型的实体框架）的异步操作。</returns>
        public virtual ValueTask<object> GetObjectStatusAsync(CancellationToken cancellationToken = default)
            => cancellationToken.RunOrCancelValueAsync(() => (object)Status);


        /// <summary>
        /// 设置对象排名。
        /// </summary>
        /// <param name="newRank">给定的新对象排名。</param>
        /// <returns>返回排名（兼容整数、单双精度的排序字段）。</returns>
        public virtual object SetObjectRank(object newRank)
        {
            Rank = newRank.CastTo<object, TRank>(nameof(newRank));
            return newRank;
        }

        /// <summary>
        /// 异步设置对象排名。
        /// </summary>
        /// <param name="newRank">给定的新对象排名。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含排名（兼容整数、单双精度的排序字段）的异步操作。</returns>
        public virtual ValueTask<object> SetObjectRankAsync(object newRank,
            CancellationToken cancellationToken = default)
        {
            var realNewRank = newRank.CastTo<object, TRank>(nameof(newRank));

            return cancellationToken.RunOrCancelValueAsync(() =>
            {
                Rank = realNewRank;
                return newRank;
            });
        }


        /// <summary>
        /// 设置对象状态。
        /// </summary>
        /// <param name="newStatus">给定的新状态对象。</param>
        /// <returns>返回状态（兼容不支持枚举类型的实体框架）。</returns>
        public virtual object SetObjectStatus(object newStatus)
        {
            Status = newStatus.CastTo<object, TStatus>(nameof(newStatus));
            return newStatus;
        }

        /// <summary>
        /// 异步设置对象状态。
        /// </summary>
        /// <param name="newStatus">给定的新状态对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含状态（兼容不支持枚举类型的实体框架）的异步操作。</returns>
        public virtual ValueTask<object> SetObjectStatusAsync(object newStatus, CancellationToken cancellationToken = default)
        {
            var realNewStatus = newStatus.CastTo<object, TStatus>(nameof(newStatus));

            return cancellationToken.RunOrCancelValueAsync(() =>
            {
                Status = realNewStatus;
                return newStatus;
            });
        }


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => $"{base.ToString()};{nameof(Rank)}={Rank};{nameof(Status)}={Status}";

    }
}
