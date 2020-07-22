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
    using Data.Accessors;
    using Data.Options;
    using Data.Stores;

    /// <summary>
    /// 数据构建器选项基类。
    /// </summary>
    public class DataBuilderOptionsBase : DataBuilderOptionsBase<DataTableOptions>
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
    /// <typeparam name="TTableOptions">指定的表名选项类型。</typeparam>
    public class DataBuilderOptionsBase<TTableOptions> : DataBuilderOptionsBase<DataStoreOptions, TTableOptions>
        where TTableOptions : AbstractTableOptions, new()
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
    public class DataBuilderOptionsBase<TStoreOptions, TTableOptions>
        : AbstractDataBuilderOptions<TStoreOptions, TTableOptions>
        where TStoreOptions : AbstractStoreOptions, new()
        where TTableOptions : AbstractTableOptions, new()
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
        /// 使用 <see cref="IStoreInitializer"/> 进行初始化（默认已启用）。
        /// </summary>
        public bool UseInitializer { get; set; }
            = true;


        /// <summary>
        /// 默认选项扩展连接字符串工厂方法。
        /// </summary>
        public Func<DbContextOptions, string> OptionsExtensionConnectionStringFactory { get; set; }
            = options => options.Extensions.OfType<RelationalOptionsExtension>().First().ConnectionString;


        /// <summary>
        /// 已创建数据库的后置动作。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Action<DbContextAccessorBase> PostDatabaseCreatedAction { get; set; }

        /// <summary>
        /// 已更改数据库的后置动作。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Action<DbContextAccessorBase> PostDatabaseChangedAction { get; set; }

        /// <summary>
        /// 已切换租户的后置动作。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Action<DbContextAccessorBase> PostTenantSwitchedAction { get; set; }

        /// <summary>
        /// 已迁移的后置动作。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Action<DbContextAccessorBase> PostMigratedAction { get; set; }

        /// <summary>
        /// 保存更改的后置动作。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Action<DbContextAccessorBase> PostSaveChangesAction { get; set; }


        /// <summary>
        /// 审计实体状态数组（默认为空表示对除 <see cref="EntityState.Unchanged"/> 外的所有实体状态进行审核）。
        /// </summary>
        public IReadOnlyList<EntityState> AuditEntityStates { get; set; }

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
