#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    using Core;
	
	/// <summary>
    /// 抽象数据构建器选项。
    /// </summary>
    /// <typeparam name="TTableSchemaOptions">指定的表架构选项类型。</typeparam>
    public abstract class AbstractDataBuilderOptions<TTableSchemaOptions>
        : AbstractDataBuilderOptions<StoreOptions, TTableSchemaOptions>
        where TTableSchemaOptions : ITableSchemaOptions, new()
	{
	}
	

    /// <summary>
    /// 抽象数据构建器选项。
    /// </summary>
    /// <typeparam name="TStoreOptions">指定的存储选项类型。</typeparam>
    /// <typeparam name="TTableSchemaOptions">指定的表架构选项类型。</typeparam>
    public abstract class AbstractDataBuilderOptions<TStoreOptions, TTableSchemaOptions>
        : AbstractBuilderOptions, IDataBuilderOptions<TStoreOptions, TTableSchemaOptions>
        where TStoreOptions : IStoreOptions, new()
        where TTableSchemaOptions : ITableSchemaOptions, new()
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
}
