#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.ComponentModel;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 分表模式。
    /// </summary>
    [Description("数据模式")]
    public enum ShardingTableMode
    {
        /// <summary>
        /// 创建分表（表示当检测到表名差异时，默认创建分表）。
        /// </summary>
        Create,

        /// <summary>
        /// 更新分表（表示当检测到表名差异时，默认重命名分表，即不分表）。
        /// </summary>
        Rename
    }
}
