#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象构建器选项。
    /// </summary>
    /// <typeparam name="TTableSchemaOptions">指定的表架构选项类型。</typeparam>
    public abstract class AbstractBuilderOptions<TTableSchemaOptions> : AbstractBuilderOptions, IBuilderOptions<TTableSchemaOptions>
        where TTableSchemaOptions : ITableSchemaOptions, new()
    {
        /// <summary>
        /// 表架构选项。
        /// </summary>
        public TTableSchemaOptions TableSchemas { get; set; }
            = new TTableSchemaOptions();
    }


    /// <summary>
    /// 抽象构建器选项。
    /// </summary>
    public abstract class AbstractBuilderOptions : IBuilderOptions
    {
        /// <summary>
        /// 选项名称。
        /// </summary>
        public string OptionsName { get; set; }
    }
}
