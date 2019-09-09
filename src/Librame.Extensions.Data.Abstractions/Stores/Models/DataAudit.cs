#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

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
    public class DataAudit : AbstractCreation<string>
    {
        /// <summary>
        /// 表名。
        /// </summary>
        [Display(Name = nameof(TableName), ResourceType = typeof(AuditResource))]
        public virtual string TableName { get; set; }

        /// <summary>
        /// 实体标识。
        /// </summary>
        [Display(Name = nameof(EntityId), ResourceType = typeof(AuditResource))]
        public virtual string EntityId { get; set; }

        /// <summary>
        /// 实体类型名。
        /// </summary>
        [Display(Name = nameof(EntityTypeName), ResourceType = typeof(AuditResource))]
        public virtual string EntityTypeName { get; set; }

        /// <summary>
        /// 操作状态。
        /// </summary>
        [Display(Name = nameof(State), ResourceType = typeof(AbstractEntityResource))]
        public virtual int State { get; set; }

        /// <summary>
        /// 状态名称。
        /// </summary>
        [Display(Name = nameof(StateName), ResourceType = typeof(AuditResource))]
        public virtual string StateName { get; set; }


        /// <summary>
        /// 实体属性集合。
        /// </summary>
        [Display(Name = nameof(Properties), ResourceType = typeof(AuditResource))]
        public virtual IList<DataAuditProperty> Properties { get; set; }
            = new List<DataAuditProperty>();
    }
}
