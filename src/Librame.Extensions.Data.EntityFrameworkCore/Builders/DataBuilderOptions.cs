#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using System;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 数据构建器选项。
    /// </summary>
    public class DataBuilderOptions : DataBuilderOptionsBase
    {
        /// <summary>
        /// 默认架构。
        /// </summary>
        public string DefaultSchema { get; set; }

        /// <summary>
        /// 是否创建数据库（如果数据库不存在；默认已启用）。
        /// </summary>
        public bool IsCreateDatabase { get; set; }
            = true;

        /// <summary>
        /// 已创建数据库动作。
        /// </summary>
        public Action<IAccessor> DatabaseCreatedAction { get; set; }


        /// <summary>
        /// 审计。
        /// </summary>
        public AuditOptions Audits { get; set; }
            = new AuditOptions();

        /// <summary>
        /// 租户。
        /// </summary>
        public TenantOptions Tenants { get; set; }
            = new TenantOptions();
    }


    /// <summary>
    /// 审计选项。
    /// </summary>
    public class AuditOptions
    {
        /// <summary>
        /// 启用审计（默认已启用）。
        /// </summary>
        public bool Enabled { get; set; }
            = true;

        /// <summary>
        /// 审计的实体状态数组（默认对实体的增加、修改、删除状态进行审核）。
        /// </summary>
        public EntityState[] AuditEntityStates { get; set; }
            = new EntityState[] { EntityState.Added, EntityState.Modified, EntityState.Deleted };
    }


    /// <summary>
    /// 租户选项。
    /// </summary>
    public class TenantOptions
    {
        /// <summary>
        /// 启用租户（默认已启用）。
        /// </summary>
        public bool Enabled { get; set; }
            = true;

        /// <summary>
        /// 默认租户。
        /// </summary>
        public ITenant Default { get; set; }
            = new Tenant();
    }
}
