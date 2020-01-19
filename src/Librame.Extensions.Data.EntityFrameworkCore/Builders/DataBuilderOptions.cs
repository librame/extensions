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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Data.Builders
{
    using Accessors;
    using Core;
    using Core.Threads;
    using Stores;

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
        /// 已创建数据库动作（默认调用 <see cref="PostChangedDbConnectionAction"/>）。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Action<DbContextAccessorBase> DatabaseCreatedAction { get; set; }
            = accessor =>
            {
                accessor.Locker.WaitAction(() => accessor.BuilderOptions.PostChangedDbConnectionAction?.Invoke(accessor));
            };

        /// <summary>
        /// 后置改变数据库连接动作。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Action<DbContextAccessorBase> PostChangedDbConnectionAction { get; set; }
            = accessor =>
            {
                if (accessor.BuilderOptions.MigrationEnabled)
                    accessor.Migrate();
            };


        /// <summary>
        /// 启用审计（默认已启用）。
        /// </summary>
        public bool AuditEnabled { get; set; }
            = true;

        /// <summary>
        /// 审计实体状态数组（默认对实体的增加、修改、删除状态进行审核）。
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public EntityState[] AuditEntityStates { get; set; }
            = new EntityState[] { EntityState.Added, EntityState.Modified, EntityState.Deleted };


        /// <summary>
        /// 启用实体注册（默认已启用）。
        /// </summary>
        public bool EntityEnabled { get; set; }
            = true;


        /// <summary>
        /// 启用迁移（默认已启用）。
        /// </summary>
        public bool MigrationEnabled { get; set; }
            = true;

        /// <summary>
        /// 迁移程序集引用列表。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public List<AssemblyReference> MigrationAssemblyReferences { get; }
            = new List<AssemblyReference>
            {
                AssemblyReference.Load("Librame.Extensions.Data.Abstractions"),
                AssemblyReference.Load("Librame.Extensions.Data.EntityFrameworkCore"),
                AssemblyReference.Load("Microsoft.EntityFrameworkCore"),
                AssemblyReference.Load("Microsoft.EntityFrameworkCore.Relational"),
                AssemblyReference.Load("netstandard"),
                AssemblyReference.Load("System.Runtime"),
                AssemblyReference.Load("System.Private.CoreLib")
            };

        /// <summary>
        /// 导出迁移程序集。
        /// </summary>
        public bool ExportMigrationAssembly { get; set; }
            = true;


        /// <summary>
        /// 数据租户（默认已启用）。
        /// </summary>
        public bool TenantEnabled { get; set; }
            = true;

        /// <summary>
        /// 默认租户。
        /// </summary>
        public ITenant DefaultTenant { get; set; }
            = new DataTenant<string>
            {
                Name = "DefaultTenant",
                Host = "localhost",
                DefaultConnectionString = "librame_data_default",
                WritingConnectionString = "librame_data_writing",
            };
    }
}
