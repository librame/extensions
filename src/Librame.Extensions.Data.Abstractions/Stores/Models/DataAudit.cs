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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Librame.Extensions.Data.Stores
{
    using Resources;

    /// <summary>
    /// 数据审计。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    [Description("数据审计")]
    [NotAudited]
    public class DataAudit<TGenId> : AbstractCreation<TGenId>
        where TGenId : IEquatable<TGenId>
    {
        /// <summary>
        /// 表名。
        /// </summary>
        [Display(Name = nameof(TableName), ResourceType = typeof(DataAuditResource))]
        public virtual string TableName { get; set; }

        /// <summary>
        /// 实体标识。
        /// </summary>
        [Display(Name = nameof(EntityId), ResourceType = typeof(DataAuditResource))]
        public virtual string EntityId { get; set; }

        /// <summary>
        /// 实体类型名。
        /// </summary>
        [Display(Name = nameof(EntityTypeName), ResourceType = typeof(DataAuditResource))]
        public virtual string EntityTypeName { get; set; }

        /// <summary>
        /// 操作状态。
        /// </summary>
        [Display(Name = nameof(State), ResourceType = typeof(AbstractEntityResource))]
        public virtual int State { get; set; }

        /// <summary>
        /// 状态名称。
        /// </summary>
        [Display(Name = nameof(StateName), ResourceType = typeof(DataAuditResource))]
        public virtual string StateName { get; set; }
    }
}
