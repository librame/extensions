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
    public abstract class AbstractDataBuilderOptions<TTableSchemaOptions> : AbstractBuilderOptions, IDataBuilderOptions<TTableSchemaOptions>
        where TTableSchemaOptions : ITableSchemaOptions, new()
    {
        /// <summary>
        /// 表架构选项。
        /// </summary>
        public TTableSchemaOptions TableSchemas { get; set; }
            = new TTableSchemaOptions();
    }
}
