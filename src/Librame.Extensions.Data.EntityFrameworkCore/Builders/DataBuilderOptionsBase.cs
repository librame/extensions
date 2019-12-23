#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Builders
{
    using Core.Builders;
    using Schemas;
    using Stores;

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
}
