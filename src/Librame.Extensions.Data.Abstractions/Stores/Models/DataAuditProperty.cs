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
    /// 数据审计属性。
    /// </summary>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TAuditId">指定的审计标识类型。</typeparam>
    [Description("数据审计属性")]
    [NonAudited]
    [Shardable]
    public class DataAuditProperty<TIncremId, TAuditId> : AbstractId<TIncremId>
        where TIncremId : IEquatable<TIncremId>
        where TAuditId : IEquatable<TAuditId>
    {
        /// <summary>
        /// 审计标识。
        /// </summary>
        [Display(Name = nameof(AuditId), ResourceType = typeof(DataAuditPropertyResource))]
        public virtual TAuditId AuditId { get; set; }

        /// <summary>
        /// 属性名称。
        /// </summary>
        [Display(Name = nameof(PropertyName), ResourceType = typeof(DataAuditPropertyResource))]
        public virtual string PropertyName { get; set; }

        /// <summary>
        /// 属性类型名称。
        /// </summary>
        [Display(Name = nameof(PropertyTypeName), ResourceType = typeof(DataAuditPropertyResource))]
        public virtual string PropertyTypeName { get; set; }

        /// <summary>
        /// 旧值。
        /// </summary>
        [Display(Name = nameof(OldValue), ResourceType = typeof(DataAuditPropertyResource))]
        public virtual string OldValue { get; set; }

        /// <summary>
        /// 新值。
        /// </summary>
        [Display(Name = nameof(NewValue), ResourceType = typeof(DataAuditPropertyResource))]
        public virtual string NewValue { get; set; }
    }
}
