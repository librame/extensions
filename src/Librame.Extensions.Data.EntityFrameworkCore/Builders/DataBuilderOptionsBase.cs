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
    /// 数据构建器选项基类。
    /// </summary>
    public class DataBuilderOptionsBase : DataBuilderOptionsBase<TableNameSchemaOptions>
    {
    }


    /// <summary>
    /// 数据构建器选项基类。
    /// </summary>
    /// <typeparam name="TTableNameSchemaOptions">指定的表名架构选项类型。</typeparam>
    public class DataBuilderOptionsBase<TTableNameSchemaOptions> : DataBuilderOptionsBase<StoreOptions, TTableNameSchemaOptions>
        where TTableNameSchemaOptions : TableNameSchemaOptions, new()
    {
    }


    /// <summary>
    /// 数据构建器选项基类。
    /// </summary>
    /// <typeparam name="TStoreOptions">指定的存储选项类型。</typeparam>
    /// <typeparam name="TTableNameSchemaOptions">指定的表名架构选项类型。</typeparam>
    public class DataBuilderOptionsBase<TStoreOptions, TTableNameSchemaOptions> : IExtensionBuilderOptions
        where TStoreOptions : StoreOptions, new()
        where TTableNameSchemaOptions : TableNameSchemaOptions, new()
    {
        /// <summary>
        /// 存储选项。
        /// </summary>
        /// <value>返回 <typeparamref name="TStoreOptions"/>。</value>
        public TStoreOptions Stores { get; set; }
            = new TStoreOptions();

        /// <summary>
        /// 表名架构选项。
        /// </summary>
        /// <value>返回 <typeparamref name="TTableNameSchemaOptions"/>。</value>
        public TTableNameSchemaOptions Tables { get; set; }
            = new TTableNameSchemaOptions();
    }


    /// <summary>
    /// 存储选项。
    /// </summary>
    public class StoreOptions
    {
        /// <summary>
        /// 启用初始化（默认已启用）。
        /// </summary>
        public bool InitializationEnabled { get; set; }
            = true;

        /// <summary>
        /// 映射关系（默认启用映射）。
        /// </summary>
        public bool MapRelationship { get; set; }
            = true;

        /// <summary>
        /// 属性的最大长度（默认不设定）。
        /// </summary>
        public int MaxLengthForProperties { get; set; }
            = 0;
    }


    /// <summary>
    /// 表名架构选项。
    /// </summary>
    public class TableNameSchemaOptions
    {
        /// <summary>
        /// 默认架构。
        /// </summary>
        public string DefaultSchema { get; set; }

        /// <summary>
        /// 默认连接符。
        /// </summary>
        public string DefaultConnector { get; set; }


        /// <summary>
        /// 内部表名前缀（如：Internal）。
        /// </summary>
        public string InternalPrefix { get; set; }
            = "Internal";

        /// <summary>
        /// 私有表名前缀（如：_）。
        /// </summary>
        public string PrivatePrefix { get; set; }
            = "_";


        /// <summary>
        /// 审计工厂方法。
        /// </summary>
        public Func<TableNameDescriptor, TableNameSchema> AuditFactory { get; set; }
            = descr => descr.AsSchema();

        /// <summary>
        /// 审计属性工厂方法。
        /// </summary>
        public Func<TableNameDescriptor, TableNameSchema> AuditPropertyFactory { get; set; }
            = descr => descr.AsSchema();

        /// <summary>
        /// 实体工厂方法。
        /// </summary>
        public Func<TableNameDescriptor, TableNameSchema> EntityFactory { get; set; }
            = descr => descr.AsSchema();

        /// <summary>
        /// 迁移工厂方法。
        /// </summary>
        public Func<TableNameDescriptor, TableNameSchema> MigrationFactory { get; set; }
            = descr => descr.AsSchema();

        /// <summary>
        /// 租户工厂方法。
        /// </summary>
        public Func<TableNameDescriptor, TableNameSchema> TenantFactory { get; set; }
            = descr => descr.AsSchema();
    }
}
