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
    /// 租户接口。
    /// </summary>
    public interface ITenant : IId<string>
    {
        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 主机。
        /// </summary>
        string Host { get; set; }

        /// <summary>
        /// 默认连接字符串。
        /// </summary>
        string DefaultConnectionString { get; set; }

        /// <summary>
        /// 写入连接字符串。
        /// </summary>
        string WriteConnectionString { get; set; }

        /// <summary>
        /// 写入分离（默认不启用）。
        /// </summary>
        bool WriteConnectionSeparation { get; set; }
    }
}
