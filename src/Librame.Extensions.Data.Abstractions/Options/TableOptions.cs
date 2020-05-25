#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Data.Options
{
    /// <summary>
    /// 表名选项。
    /// </summary>
    public class TableOptions
    {
        /// <summary>
        /// 默认架构。
        /// </summary>
        public string DefaultSchema { get; set; }

        /// <summary>
        /// 默认连接符。
        /// </summary>
        public string DefaultConnector { get; set; }

        /// <summary>
        /// 使用 Data 前缀（默认使用）。
        /// </summary>
        public bool UseDataPrefix { get; set; }
            = true;
        

        /// <summary>
        /// 审计表描述符配置动作。
        /// </summary>
        public Action<TableDescriptor> Audit { get; set; }

        /// <summary>
        /// 审计属性表描述符配置动作。
        /// </summary>
        public Action<TableDescriptor> AuditProperty { get; set; }

        /// <summary>
        /// 实体表描述符配置动作。
        /// </summary>
        public Action<TableDescriptor> Entity { get; set; }

        /// <summary>
        /// 迁移表描述符配置动作。
        /// </summary>
        public Action<TableDescriptor> Migration { get; set; }

        /// <summary>
        /// 租户表描述符配置动作。
        /// </summary>
        public Action<TableDescriptor> Tenant { get; set; }
    }
}
