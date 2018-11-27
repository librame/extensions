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

namespace Librame.Extensions.Data
{
    using Builders;

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
        /// 启用审计（默认已启用）。
        /// </summary>
        public bool AuditEnabled { get; set; } = true;

        /// <summary>
        /// 启用租户（默认已启用）。
        /// </summary>
        public bool TenantEnabled { get; set; } = true;

        /// <summary>
        /// 确保数据库已创建（默认已启用）。
        /// </summary>
        public bool EnsureDbCreated { get; set; } = true;

        /// <summary>
        /// 发布审计事件动作方法。
        /// </summary>
        public Action<IDbProvider, IList<Audit>> PublishAuditEvent { get; set; }


        /// <summary>
        /// 本机租户。
        /// </summary>
        public Tenant LocalTenant { get; set; } = new Tenant
        {
            Id = Guid.Empty.ToString(),
            Name = "Local",
            Host = "localhost"
        };


        /// <summary>
        /// 审计表选项。
        /// </summary>
        public ITableSchema AuditTable { get; set; }

        /// <summary>
        /// 审计属性表选项。
        /// </summary>
        public IShardingSchema AuditPropertyTable { get; set; }
        
        /// <summary>
        /// 租户表选项。
        /// </summary>
        public ITableSchema TenantTable { get; set; }
    }

}
