#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
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
    using Core.Builders;
    using Data.Accessors;
    using Data.Options;
    using Data.Stores;

    /// <summary>
    /// 数据构建器选项基类。
    /// </summary>
    public class DataBuilderOptionsBase : DataBuilderOptionsBase<TableOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="DataBuilderOptionsBase"/>
        /// </summary>
        /// <param name="defaultTenant">给定的默认 <see cref="ITenant"/>。</param>
        public DataBuilderOptionsBase(ITenant defaultTenant)
            : base(defaultTenant)
        {
        }
    }


    /// <summary>
    /// 数据构建器选项基类。
    /// </summary>
    /// <typeparam name="TTableOptions">指定的表选项类型。</typeparam>
    public class DataBuilderOptionsBase<TTableOptions> : DataBuilderOptionsBase<StoreOptions, TTableOptions>
        where TTableOptions : TableOptions, new()
    {
        /// <summary>
        /// 构造一个 <see cref="DataBuilderOptionsBase{TTableOptions}"/>
        /// </summary>
        /// <param name="defaultTenant">给定的默认 <see cref="ITenant"/>。</param>
        public DataBuilderOptionsBase(ITenant defaultTenant)
            : base(defaultTenant)
        {
        }
    }


    /// <summary>
    /// 数据构建器选项基类。
    /// </summary>
    /// <typeparam name="TStoreOptions">指定的存储选项类型。</typeparam>
    /// <typeparam name="TTableOptions">指定的表名选项类型。</typeparam>
    public class DataBuilderOptionsBase<TStoreOptions, TTableOptions> : IExtensionBuilderOptions
        where TStoreOptions : StoreOptions, new()
        where TTableOptions : TableOptions, new()
    {
        /// <summary>
        /// 构造一个 <see cref="DataBuilderOptionsBase{TStoreOptions, TTableOptions}"/>
        /// </summary>
        /// <param name="defaultTenant">给定的默认 <see cref="ITenant"/>。</param>
        public DataBuilderOptionsBase(ITenant defaultTenant)
        {
            DefaultTenant = StoreHelper.PopulateDefaultTenant(defaultTenant);
        }


        /// <summary>
        /// 默认租户。
        /// </summary>
        public ITenant DefaultTenant { get; set; }

        /// <summary>
        /// 如果数据库不存在时，支持创建数据库（默认已启用）。
        /// </summary>
        public bool SupportsCreateDatabase { get; set; }
            = true;


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


        /// <summary>
        /// 存储选项。
        /// </summary>
        /// <value>返回 <typeparamref name="TStoreOptions"/>。</value>
        public TStoreOptions Stores { get; }
            = new TStoreOptions();

        /// <summary>
        /// 表选项。
        /// </summary>
        /// <value>返回 <typeparamref name="TTableOptions"/>。</value>
        public TTableOptions Tables { get; }
            = new TTableOptions();
    }
}
