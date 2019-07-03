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
    /// <typeparam name="TTableSchemaOptions">指定的表架构选项类型。</typeparam>
    public interface IDataBuilderOptions<TTableSchemaOptions> : IBuilderOptions
        where TTableSchemaOptions : ITableSchemaOptions
    {
        /// <summary>
        /// 表架构选项。
        /// </summary>
        TTableSchemaOptions TableSchemas { get; set; }
    }
}
