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

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 抽象实体。
    /// </summary>
    public abstract class AbstractEntity : AbstractEntity<float, DataStatus>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractEntity"/> 默认实例。
        /// </summary>
        public AbstractEntity()
        {
            Rank = 10;
            Status = DataStatus.Public;
        }
    }


    /// <summary>
    /// 抽象实体（默认以字符串为标识类型）。
    /// </summary>
    /// <typeparam name="TRank">指定的排序类型（兼容整数、单双精度的排序字段）。</typeparam>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    public abstract class AbstractEntity<TRank, TStatus> : AbstractEntity<string, TRank, TStatus>
        where TRank : struct
        where TStatus : struct
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractEntity{TRank, TStatus}"/> 默认实例。
        /// </summary>
        public AbstractEntity()
        {
            // 默认使用空标识符，新增推荐使用服务注入
            Id = GuIdentifier.Empty;
        }
    }


    /// <summary>
    /// 抽象实体。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    public abstract class AbstractEntity<TId> : AbstractEntity<TId, float, DataStatus>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractEntity{TId}"/> 默认实例。
        /// </summary>
        public AbstractEntity()
        {
            Rank = 10;
            Status = DataStatus.Public;
        }
    }


    /// <summary>
    /// 抽象实体。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TRank">指定的排序类型（兼容整数、单双精度的排序字段）。</typeparam>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    public abstract class AbstractEntity<TId, TRank, TStatus> : AbstractId<TId>, IRank<TRank>, IStatus<TStatus>
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
    }
}
