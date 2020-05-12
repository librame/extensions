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
    using Data.Accessors;
    using Data.Stores;

    /// <summary>
    /// 数据构建器选项。
    /// </summary>
    public class DataBuilderOptions : DataBuilderOptionsBase
    {
        /// <summary>
        /// 构造一个 <see cref="DataBuilderOptions"/>。
        /// </summary>
        public DataBuilderOptions()
            : base(new DataTenant<Guid>())
        {
        }


        /// <summary>
        /// 默认选项扩展连接字符串工厂方法。
        /// </summary>
        public Func<DbContextOptions, string> OptionsExtensionConnectionStringFactory { get; set; }
            = options => options.Extensions.OfType<RelationalOptionsExtension>().First().ConnectionString;

        /// <summary>
        /// 改变数据库连接的后置动作（默认调用 <see cref="DbContextAccessorBase.Migrate()"/>）。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Action<DbContextAccessorBase> PostChangedDbConnectionAction { get; set; }
            = accessor => accessor.Migrate();

        /// <summary>
        /// 创建数据库的后置动作（默认调用 <see cref="PostChangedDbConnectionAction"/>）。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Action<DbContextAccessorBase> PostDatabaseCreatedAction { get; set; }
            = accessor => accessor.Dependency.Options.PostChangedDbConnectionAction?.Invoke(accessor);


        /// <summary>
        /// 审计实体状态数组（默认对实体的增加、修改、删除状态进行审核）。
        /// </summary>
        public IReadOnlyList<EntityState> AuditEntityStates { get; set; }
            = new List<EntityState> { EntityState.Added, EntityState.Modified, EntityState.Deleted };

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
    }
}
