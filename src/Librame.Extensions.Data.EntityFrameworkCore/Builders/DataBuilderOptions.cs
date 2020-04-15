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
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Data.Builders
{
    using Core;
    using Core.Identifiers;
    using Data.Accessors;
    using Data.Stores;

    /// <summary>
    /// 数据构建器选项。
    /// </summary>
    public class DataBuilderOptions : DataBuilderOptionsBase
    {
        /// <summary>
        /// 有序唯一标识符生成器方案（默认使用符合 SQL Server 规则的有序 <see cref="Guid"/>）。
        /// </summary>
        public SequentialUniqueIdentifierGenerator SUIDGenerator { get; set; }
            = SequentialUniqueIdentifierGenerator.SqlServer;
        
        /// <summary>
        /// 默认选项扩展连接字符串工厂方法。
        /// </summary>
        public Func<DbContextOptions, string> OptionsExtensionConnectionStringFactory { get; set; }
            = options => options.Extensions.OfType<RelationalOptionsExtension>().First().ConnectionString;

        /// <summary>
        /// 如果数据库不存在时，是否创建数据库（默认已启用）。
        /// </summary>
        public bool IsCreateDatabase { get; set; }
            = true;

        /// <summary>
        /// 创建数据库的后置动作（默认调用 <see cref="PostChangedDbConnectionAction"/>）。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Action<DbContextAccessorBase> PostDatabaseCreatedAction { get; set; }
            = accessor => accessor.BuilderOptions.PostChangedDbConnectionAction?.Invoke(accessor);

        /// <summary>
        /// 改变数据库连接的后置动作（默认调用 <see cref="DbContextAccessorBase.Migrate()"/>）。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Action<DbContextAccessorBase> PostChangedDbConnectionAction { get; set; }
            = accessor => accessor.Migrate();


        /// <summary>
        /// 启用审计（默认已启用）。
        /// </summary>
        public bool AuditEnabled { get; set; }
            = true;

        /// <summary>
        /// 审计实体状态数组（默认对实体的增加、修改、删除状态进行审核）。
        /// </summary>
        public IReadOnlyList<EntityState> AuditEntityStates { get; set; }
            = new List<EntityState> { EntityState.Added, EntityState.Modified, EntityState.Deleted };


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
        /// 导出迁移命令集合。
        /// </summary>
        public bool ExportMigrationCommands { get; set; }
            = true;

        /// <summary>
        /// 模型缓存过期秒数。
        /// </summary>
        public int ModelCacheExpirationSeconds { get; set; }
            = 5;


        /// <summary>
        /// 数据租户（默认已启用）。
        /// </summary>
        public bool TenantEnabled { get; set; }
            = true;

        /// <summary>
        /// 默认租户（默认使用 <see cref="Guid"/> 生成式标识类型）。
        /// </summary>
        public ITenant DefaultTenant { get; set; }
            = new DataTenant<Guid>
            {
                Name = "DefaultTenant",
                Host = "localhost",
                DefaultConnectionString = "librame_data_default",
                WritingConnectionString = "librame_data_writing",
            };
    }
}
