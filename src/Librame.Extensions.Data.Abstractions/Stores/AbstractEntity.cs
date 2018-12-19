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
    /// <summary>
    /// 抽象实体。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TDateTime">指定的日期与时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    public abstract class AbstractEntity<TId, TDateTime, TStatus> : AbstractId<TId>, IEntity<TId, TDateTime, TStatus>
        where TId : IEquatable<TId>
        where TDateTime : struct
        where TStatus : struct
    {
        /// <summary>
        /// 数据排序。
        /// </summary>
        [Display(Name = nameof(DataRank), GroupName = "DataGroup", ResourceType = typeof(AbstractEntityResource))]
        public virtual int DataRank { get; set; }

        /// <summary>
        /// 数据状态。
        /// </summary>
        [Display(Name = nameof(DataStatus), GroupName = "DataGroup", ResourceType = typeof(AbstractEntityResource))]
        public virtual TStatus DataStatus { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        [Display(Name = nameof(CreateTime), GroupName = "CreateGroup", ResourceType = typeof(AbstractEntityResource))]
        [DataType(DataType.DateTime)]
        public virtual TDateTime CreateTime { get; set; }

        /// <summary>
        /// 创建者标识。
        /// </summary>
        [Display(Name = nameof(CreatorId), GroupName = "CreateGroup", ResourceType = typeof(AbstractEntityResource))]
        public virtual TId CreatorId { get; set; }

        /// <summary>
        /// 更新时间。
        /// </summary>
        [Display(Name = nameof(UpdateTime), GroupName = "UpdateGroup", ResourceType = typeof(AbstractEntityResource))]
        [DataType(DataType.DateTime)]
        public virtual TDateTime UpdateTime { get; set; }

        /// <summary>
        /// 更新者标识。
        /// </summary>
        [Display(Name = nameof(UpdatorId), GroupName = "UpdateGroup", ResourceType = typeof(AbstractEntityResource))]
        public virtual TId UpdatorId { get; set; }
    }


    /// <summary>
    /// 抽象实体。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TTenantId">指定的租户标识类型。</typeparam>
    /// <typeparam name="TDateTime">指定的日期与时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    public abstract class AbstractEntity<TId, TTenantId, TDateTime, TStatus> : AbstractEntity<TId, TDateTime, TStatus>, ITenantId<TTenantId>
        where TId : IEquatable<TId>
        where TTenantId : IEquatable<TTenantId>
        where TDateTime : struct
        where TStatus : struct
    {
        /// <summary>
        /// 租户标识。
        /// </summary>
        [Display(Name = nameof(TenantId), GroupName = "GlobalGroup", ResourceType = typeof(AbstractEntityResource))]
        public TTenantId TenantId { get; set; }
    }
}
