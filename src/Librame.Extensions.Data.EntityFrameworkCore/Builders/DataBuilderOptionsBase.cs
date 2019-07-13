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
        : AbstractDataBuilderOptions<StoreOptions, TTableSchemaOptions>
        where TTableSchemaOptions : ITableSchemaOptions, new()
    {
    }
}
