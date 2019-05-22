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
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 基础审计。
    /// </summary>
    [NotAudited]
    [Description("基础审计")]
    public class BaseAudit : AbstractId
    {
        /// <summary>
        /// 实体标识。
        /// </summary>
        [Display(Name = nameof(EntityId), ResourceType = typeof(BaseAuditResource))]
        public virtual string EntityId { get; set; }

        /// <summary>
        /// 实体名称。
        /// </summary>
        [Display(Name = nameof(EntityName), ResourceType = typeof(BaseAuditResource))]
        public virtual string EntityName { get; set; }

        /// <summary>
        /// 实体类型名称。
        /// </summary>
        [Display(Name = nameof(EntityTypeName), ResourceType = typeof(BaseAuditResource))]
        public virtual string EntityTypeName { get; set; }

        /// <summary>
        /// 操作状态。
        /// </summary>
        [Display(Name = nameof(State), ResourceType = typeof(BaseAuditResource))]
        public virtual int State { get; set; }

        /// <summary>
        /// 状态名称。
        /// </summary>
        [Display(Name = nameof(StateName), ResourceType = typeof(BaseAuditResource))]
        public virtual string StateName { get; set; }

        /// <summary>
        /// 创建者。
        /// </summary>
        [Display(Name = nameof(CreatedBy), ResourceType = typeof(BaseAuditResource))]
        public virtual string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        [Display(Name = nameof(CreatedTime), ResourceType = typeof(BaseAuditResource))]
        public virtual DateTimeOffset CreatedTime { get; set; }


        /// <summary>
        /// 实体属性集合。
        /// </summary>
        [Display(Name = nameof(Properties), ResourceType = typeof(BaseAuditResource))]
        public virtual IList<BaseAuditProperty> Properties { get; set; }
            = new List<BaseAuditProperty>();
    }
}
