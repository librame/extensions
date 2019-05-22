#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 基础审计属性。
    /// </summary>
    [NotAudited]
    [Description("基础审计属性")]
    public class BaseAuditProperty : AbstractId<int>
    {
        /// <summary>
        /// 属性名称。
        /// </summary>
        [Display(Name = nameof(PropertyName), ResourceType = typeof(BaseAuditPropertyResource))]
        public virtual string PropertyName { get; set; }

        /// <summary>
        /// 属性类型名称。
        /// </summary>
        [Display(Name = nameof(PropertyTypeName), ResourceType = typeof(BaseAuditPropertyResource))]
        public virtual string PropertyTypeName { get; set; }

        /// <summary>
        /// 旧值。
        /// </summary>
        [Display(Name = nameof(OldValue), ResourceType = typeof(BaseAuditPropertyResource))]
        public virtual string OldValue { get; set; }

        /// <summary>
        /// 新值。
        /// </summary>
        [Display(Name = nameof(NewValue), ResourceType = typeof(BaseAuditPropertyResource))]
        public virtual string NewValue { get; set; }

        /// <summary>
        /// 审计标识。
        /// </summary>
        [Display(Name = nameof(AuditId), ResourceType = typeof(BaseAuditPropertyResource))]
        public virtual string AuditId { get; set; }


        /// <summary>
        /// 基础审计。
        /// </summary>
        public virtual BaseAudit Audit { get; set; }
    }
}
