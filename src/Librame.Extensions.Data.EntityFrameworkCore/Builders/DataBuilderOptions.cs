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
        /// 是否创建数据库（如果数据库不存在；默认已启用）。
        /// </summary>
        public bool IsCreateDatabase { get; set; }
            = true;

        /// <summary>
        /// 已创建数据库动作。
        /// </summary>
        public Action<IAccessor> DatabaseCreatedAction { get; set; }


        /// <summary>
        /// 是 UTC 时钟。
        /// </summary>
        public bool IsUtcClock { get; set; }


        /// <summary>
        /// 启用审计（默认已启用）。
        /// </summary>
        public bool AuditEnabled { get; set; }
            = true;

        /// <summary>
        /// 审计的实体状态数组（默认对实体的增加、修改、删除状态进行审核）。
        /// </summary>
        public EntityState[] AuditEntityStates { get; set; }
            = new EntityState[] { EntityState.Added, EntityState.Modified, EntityState.Deleted };


        /// <summary>
        /// 启用实体表（默认已启用）。
        /// </summary>
        public bool TableEnabled { get; set; }
            = true;


        /// <summary>
        /// 启用租户（默认已启用）。
        /// </summary>
        public bool TenantEnabled { get; set; }
            = true;

        /// <summary>
        /// 默认租户。
        /// </summary>
        public ITenant DefaultTenant { get; set; }
            = new DataTenant
            {
                Name = "DefaultTenant",
                Host = "localhost",
                DefaultConnectionString = "librame_data_default",
                WritingConnectionString = "librame_data_writing",
            };
    }
}
