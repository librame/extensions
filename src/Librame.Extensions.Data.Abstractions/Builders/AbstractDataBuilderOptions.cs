#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Builders
{
    using Options;

    /// <summary>
    /// 抽象数据构建器选项。
    /// </summary>
    /// <typeparam name="TStoreOptions">指定的存储选项类型。</typeparam>
    /// <typeparam name="TTableOptions">指定的表名选项类型。</typeparam>
    public abstract class AbstractDataBuilderOptions<TStoreOptions, TTableOptions>
        where TStoreOptions : AbstractStoreOptions, new()
        where TTableOptions : AbstractTableOptions, new()
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
