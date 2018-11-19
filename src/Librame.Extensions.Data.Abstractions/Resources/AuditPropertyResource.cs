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
    /// 审计属性资源。
    /// </summary>
    public class AuditPropertyResource : Resources.IResource
    {
        /// <summary>
        /// 属性名称。
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 属性类型名称。
        /// </summary>
        public string PropertyTypeName { get; set; }

        /// <summary>
        /// 旧值。
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// 新值。
        /// </summary>
        public string NewValue { get; set; }

        /// <summary>
        /// 审计标识。
        /// </summary>
        public string AuditId { get; set; }

        /// <summary>
        /// 审计。
        /// </summary>
        public string Audit { get; set; }
    }
}
