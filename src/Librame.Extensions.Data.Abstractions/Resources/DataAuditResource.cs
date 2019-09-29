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
    using Core;

    /// <summary>
    /// 数据审计资源。
    /// </summary>
    public class DataAuditResource : IResource
    {
        /// <summary>
        /// 表名。
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 实体标识。
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// 实体类型名。
        /// </summary>
        public string EntityTypeName { get; set; }

        /// <summary>
        /// 状态名称。
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// 属性集合。
        /// </summary>
        public string Properties { get; set; }
    }
}
