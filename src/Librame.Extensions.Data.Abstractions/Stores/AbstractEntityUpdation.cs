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
using System.Globalization;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 抽象实体更新。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    [NotMapped]
    public abstract class AbstractEntityUpdation<TId> : AbstractEntityUpdation<TId, string, DateTimeOffset>, IUpdatedTimeTicks
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractEntityUpdation{TId}"/>。
        /// </summary>
        public AbstractEntityUpdation()
        {
            UpdatedTime = CreatedTime = DataDefaults.UtcNowOffset;
            UpdatedTimeTicks = CreatedTimeTicks = CreatedTime.Ticks.ToString(CultureInfo.InvariantCulture);
        }


        /// <summary>
        /// 创建时间周期数。
        /// </summary>
        [Display(Name = nameof(CreatedTimeTicks), ResourceType = typeof(AbstractEntityResource))]
        public virtual string CreatedTimeTicks { get; set; }

        /// <summary>
        /// 更新时间周期数。
        /// </summary>
        [Display(Name = nameof(UpdatedTimeTicks), ResourceType = typeof(AbstractEntityResource))]
        public virtual string UpdatedTimeTicks { get; set; }
    }


    /// <summary>
    /// 抽象实体更新。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的更新者类型。</typeparam>
    /// <typeparam name="TCreatedTime">指定的更新时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
    [NotMapped]
    public abstract class AbstractEntityUpdation<TId, TCreatedBy, TCreatedTime> : AbstractEntityUpdation<TId, float, DataStatus, TCreatedBy, TCreatedTime>
        where TId : IEquatable<TId>
        where TCreatedBy : IEquatable<TCreatedBy>
        where TCreatedTime : struct
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractEntityUpdation{TId, TCreatedBy, TCreatedTime}"/>。
        /// </summary>
        public AbstractEntityUpdation()
        {
            Rank = DataDefaults.Rank;
            Status = DataDefaults.Status;
        }
    }


    /// <summary>
    /// 抽象实体更新。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TRank">指定的排序类型（兼容整数、单双精度的排序字段）。</typeparam>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    /// <typeparam name="TCreatedBy">指定的更新者类型。</typeparam>
    /// <typeparam name="TCreatedTime">指定的更新时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
    [NotMapped]
    public abstract class AbstractEntityUpdation<TId, TRank, TStatus, TCreatedBy, TCreatedTime> : AbstractUpdation<TId, TCreatedBy, TCreatedTime>, ISortable<TRank>, IStatus<TStatus>
        where TId : IEquatable<TId>
        where TRank : struct
        where TStatus : struct
        where TCreatedBy : IEquatable<TCreatedBy>
        where TCreatedTime : struct
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
    }
}
