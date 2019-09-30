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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 分表特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,
        AllowMultiple = false, Inherited = false)]
    public class ShardingTableAttribute : Attribute
    {
        /// <summary>
        /// 构造一个 <see cref="ShardingTableAttribute"/>。
        /// </summary>
        /// <param name="mode">给它的 <see cref="ShardingTableMode"/>（可选；默认模式为创建新表）。</param>
        public ShardingTableAttribute(ShardingTableMode mode = ShardingTableMode.Create)
            : base()
        {
            Mode = mode;
        }


        /// <summary>
        /// 分表模式。
        /// </summary>
        public ShardingTableMode Mode { get; set; }
    }
}
