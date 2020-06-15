#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Options
{
    /// <summary>
    /// 抽象存储选项。
    /// </summary>
    public abstract class AbstractTableOptions
    {
        /// <summary>
        /// 默认架构。
        /// </summary>
        public string DefaultSchema { get; set; }

        /// <summary>
        /// 默认连接符。
        /// </summary>
        public string DefaultConnector { get; set; }
    }
}
