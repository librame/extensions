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

namespace Librame.Extensions.Data.Builders
{
    using Core.Builders;
    using Core.Identifiers;
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
            DefaultTenant = EntityPopulator.PopulateDefaultTenant(defaultTenant);
        }


        /// <summary>
        /// 默认租户。
        /// </summary>
        public ITenant DefaultTenant { get; set; }

        /// <summary>
        /// COMB 标识符生成器方案（默认使用符合 SQL Server 排序规则的有序 <see cref="Guid"/> 标识）。
        /// </summary>
        public CombIdentifierGenerator IdentifierGenerator { get; set; }
            = CombIdentifierGenerator.SQLServer;

        /// <summary>
        /// 如果数据库不存在时，支持创建数据库（默认已启用）。
        /// </summary>
        public bool SupportsCreateDatabase { get; set; }
            = true;


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
