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
    public class DataBuilderOptionsBase : DataBuilderOptionsBase<TableSchemaOptions>
    {
    }


    /// <summary>
    /// 数据构建器选项基类。
    /// </summary>
    /// <typeparam name="TTableSchemaOptions">指定的表架构选项类型。</typeparam>
    public class DataBuilderOptionsBase<TTableSchemaOptions>
        : DataBuilderOptionsBase<StoreOptions, TTableSchemaOptions>
        where TTableSchemaOptions : TableSchemaOptions, new()
    {
    }


    /// <summary>
    /// 数据构建器选项基类。
    /// </summary>
    /// <typeparam name="TStoreOptions">指定的存储选项类型。</typeparam>
    /// <typeparam name="TTableSchemaOptions">指定的表架构选项类型。</typeparam>
    public class DataBuilderOptionsBase<TStoreOptions, TTableSchemaOptions>
        : AbstractExtensionBuilderOptions
        where TStoreOptions : StoreOptions, new()
        where TTableSchemaOptions : TableSchemaOptions, new()
    {
        /// <summary>
        /// 存储选项。
        /// </summary>
        /// <value>返回 <typeparamref name="TStoreOptions"/>。</value>
        public TStoreOptions Stores { get; set; }
            = new TStoreOptions();

        /// <summary>
        /// 表架构选项。
        /// </summary>
        /// <value>返回 <typeparamref name="TTableSchemaOptions"/>。</value>
        public TTableSchemaOptions TableSchemas { get; set; }
            = new TTableSchemaOptions();
    }


    /// <summary>
    /// 存储选项。
    /// </summary>
    public class StoreOptions
    {
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
    /// 表架构选项。
    /// </summary>
    public class TableSchemaOptions
    {
        /// <summary>
        /// 审计工厂方法。
        /// </summary>
        public Func<Type, ITableSchema> AuditFactory { get; set; }
            = type => type.AsInternalTableSchema();

        /// <summary>
        /// 审计属性工厂方法。
        /// </summary>
        public Func<Type, ITableSchema> AuditPropertyFactory { get; set; }
            = type => type.AsInternalTableSchema();

        /// <summary>
        /// 租户工厂方法。
        /// </summary>
        public Func<Type, ITableSchema> TenantFactory { get; set; }
            = type => type.AsInternalTableSchema();
    }
}
