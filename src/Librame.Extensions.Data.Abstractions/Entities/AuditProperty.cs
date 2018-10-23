#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 审计属性。
    /// </summary>
    [NotAudited]
    public class AuditProperty : AbstractId<int>
    {
        /// <summary>
        /// 属性名。
        /// </summary>
        public virtual string PropertyName { get; set; }

        /// <summary>
        /// 属性类型名。
        /// </summary>
        public virtual string PropertyTypeName { get; set; }

        /// <summary>
        /// 旧值。
        /// </summary>
        public virtual string OldValue { get; set; }

        /// <summary>
        /// 新值。
        /// </summary>
        public virtual string NewValue { get; set; }

        /// <summary>
        /// 审计标识。
        /// </summary>
        public virtual int AuditId { get; set; }

        /// <summary>
        /// 审计。
        /// </summary>
        public virtual Audit Audit { get; set; }
    }
}
