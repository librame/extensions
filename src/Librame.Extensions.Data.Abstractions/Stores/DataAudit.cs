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
    /// 数据审计。
    /// </summary>
    [NotAudited]
    [Description("数据审计")]
    public class DataAudit : AbstractId
    {
        /// <summary>
        /// 实体标识。
        /// </summary>
        [Display(Name = nameof(EntityId), ResourceType = typeof(DataAuditResource))]
        public virtual string EntityId { get; set; }

        /// <summary>
        /// 实体名称。
        /// </summary>
        [Display(Name = nameof(EntityName), ResourceType = typeof(DataAuditResource))]
        public virtual string EntityName { get; set; }

        /// <summary>
        /// 实体类型名称。
        /// </summary>
        [Display(Name = nameof(EntityTypeName), ResourceType = typeof(DataAuditResource))]
        public virtual string EntityTypeName { get; set; }

        /// <summary>
        /// 操作状态。
        /// </summary>
        [Display(Name = nameof(State), ResourceType = typeof(DataAuditResource))]
        public virtual int State { get; set; }

        /// <summary>
        /// 状态名称。
        /// </summary>
        [Display(Name = nameof(StateName), ResourceType = typeof(DataAuditResource))]
        public virtual string StateName { get; set; }

        /// <summary>
        /// 创建者。
        /// </summary>
        [Display(Name = nameof(CreatedBy), ResourceType = typeof(DataAuditResource))]
        public virtual string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        [Display(Name = nameof(CreatedTime), ResourceType = typeof(DataAuditResource))]
        public virtual DateTimeOffset CreatedTime { get; set; }


        /// <summary>
        /// 实体属性集合。
        /// </summary>
        [Display(Name = nameof(Properties), ResourceType = typeof(DataAuditResource))]
        public virtual IList<DataAuditProperty> Properties { get; set; }
            = new List<DataAuditProperty>();
    }
}
