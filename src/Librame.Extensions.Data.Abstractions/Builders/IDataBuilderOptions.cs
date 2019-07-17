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
    /// 数据构建器选项接口。
    /// </summary>
    /// <typeparam name="TStoreOptions">指定的存储选项类型。</typeparam>
    /// <typeparam name="TTableSchemaOptions">指定的表架构选项类型。</typeparam>
    public interface IDataBuilderOptions<TStoreOptions, TTableSchemaOptions> : IExtensionBuilderOptions
        where TStoreOptions : IStoreOptions
        where TTableSchemaOptions : ITableSchemaOptions
    {
        /// <summary>
        /// 存储选项。
        /// </summary>
        /// <value>返回 <typeparamref name="TStoreOptions"/>。</value>
        TStoreOptions Stores { get; set; }

        /// <summary>
        /// 表架构选项。
        /// </summary>
        /// <value>返回 <typeparamref name="TTableSchemaOptions"/>。</value>
        TTableSchemaOptions TableSchemas { get; set; }
    }
}
