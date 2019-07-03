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

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 数据构建器选项。
    /// </summary>
    public class DataBuilderOptions : AbstractDataBuilderOptions<TableSchemaOptions>
    {
        /// <summary>
        /// 默认架构。
        /// </summary>
        public string DefaultSchema { get; set; }

        /// <summary>
        /// 启用审计（默认已启用）。
        /// </summary>
        public bool AuditEnabled { get; set; }
            = true;

        /// <summary>
        /// 启用租户（默认已启用）。
        /// </summary>
        public bool TenantEnabled { get; set; }
            = true;

        /// <summary>
        /// 确保数据库（默认已启用）。
        /// </summary>
        public bool EnsureDatabase { get; set; }
            = true;


        /// <summary>
        /// 本机租户。
        /// </summary>
        public ITenant DefaultTenant { get; set; }
            = new BaseTenant();
    }


    /// <summary>
    /// 表架构选项。
    /// </summary>
    public class TableSchemaOptions : ITableSchemaOptions
    {
        /// <summary>
        /// 审计工厂方法。
        /// </summary>
        public Func<Type, ITableSchema> AuditFactory { get; set; }
            = type => type.AsInternalTableSchema();

        /// <summary>
        /// 审计属性工厂方法。
        /// </summary>
        public Func<Type, ITableSchema> AuditPropertyFactory { get; set; }
            = type => type.AsInternalTableSchema();

        /// <summary>
        /// 租户工厂方法。
        /// </summary>
        public Func<Type, ITableSchema> TenantFactory { get; set; }
            = type => type.AsInternalTableSchema();
    }
}
