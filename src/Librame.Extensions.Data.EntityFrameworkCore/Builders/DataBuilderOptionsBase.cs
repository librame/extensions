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
    using Data.Stores;

    /// <summary>
    /// 数据构建器选项基类。
    /// </summary>
    public class DataBuilderOptionsBase : DataBuilderOptionsBase<TableOptions>
    {
    }


    /// <summary>
    /// 数据构建器选项基类。
    /// </summary>
    /// <typeparam name="TTableOptions">指定的表选项类型。</typeparam>
    public class DataBuilderOptionsBase<TTableOptions> : DataBuilderOptionsBase<StoreOptions, TTableOptions>
        where TTableOptions : TableOptions, new()
    {
    }


    /// <summary>
    /// 数据构建器选项基类。
    /// </summary>
    /// <typeparam name="TStoreOptions">指定的存储选项类型。</typeparam>
    /// <typeparam name="TTableOptions">指定的表名架构选项类型。</typeparam>
    public class DataBuilderOptionsBase<TStoreOptions, TTableOptions> : IExtensionBuilderOptions
        where TStoreOptions : StoreOptions, new()
        where TTableOptions : TableOptions, new()
    {
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
