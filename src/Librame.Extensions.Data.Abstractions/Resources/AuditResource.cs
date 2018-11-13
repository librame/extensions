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
    using Resources;

    /// <summary>
    /// 审计资源。
    /// </summary>
    public class AuditResource : IResource
    {
        /// <summary>
        /// 实体标识。
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// 实体名称。
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// 实体类型名称。
        /// </summary>
        public string EntityTypeName { get; set; }

        /// <summary>
        /// 状态。
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 状态名称。
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// 创建者。
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        public string CreatedTime { get; set; }

        /// <summary>
        /// 属性集合。
        /// </summary>
        public string Properties { get; set; }
    }
}
