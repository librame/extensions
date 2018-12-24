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

namespace Librame.Builders
{
    using Extensions.Data;

    /// <summary>
    /// 数据构建器选项。
    /// </summary>
    public class DataBuilderOptions : IBuilderOptions
    {
        /// <summary>
        /// 默认架构。
        /// </summary>
        public string DefaultSchema { get; set; }

        /// <summary>
        /// 默认快照名称。
        /// </summary>
        public string DefaultSnapshotName { get; set; }

        /// <summary>
        /// 启用审计（默认已启用）。
        /// </summary>
        public bool AuditEnabled { get; set; } = true;

        /// <summary>
        /// 启用租户（默认已启用）。
        /// </summary>
        public bool TenantEnabled { get; set; } = true;

        /// <summary>
        /// 确保数据库（默认已启用）。
        /// </summary>
        public bool EnsureDatabase { get; set; } = true;

        /// <summary>
        /// 发布审计事件动作方法。
        /// </summary>
        public Action<IDbProvider, IList<Audit>> PublishAuditEvent { get; set; }


        /// <summary>
        /// 本机租户。
        /// </summary>
        public ITenant LocalTenant { get; set; } = new Tenant
        {
            Id = Guid.Empty.ToString(),
            Name = "Local",
            Host = "localhost"
        };


        /// <summary>
        /// 审计表。
        /// </summary>
        public ITableSchema AuditTable { get; set; }

        /// <summary>
        /// 审计属性表。
        /// </summary>
        public ITableSchema AuditPropertyTable { get; set; }

        /// <summary>
        /// 迁移审计表。
        /// </summary>
        public ITableSchema MigrationAuditTable { get; set; }

        /// <summary>
        /// 租户表。
        /// </summary>
        public ITableSchema TenantTable { get; set; }
    }

}
