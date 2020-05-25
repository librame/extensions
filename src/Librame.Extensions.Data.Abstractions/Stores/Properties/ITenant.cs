#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 租户接口。
    /// </summary>
    public interface ITenant : IEquatable<ITenant>
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
        /// 加密连接字符串集合。
        /// </summary>
        bool EncryptedConnectionStrings { get; set; }

        /// <summary>
        /// 默认连接字符串。
        /// </summary>
        string DefaultConnectionString { get; set; }

        /// <summary>
        /// 写入连接字符串。
        /// </summary>
        string WritingConnectionString { get; set; }

        /// <summary>
        /// 写入分离（默认不启用）。
        /// </summary>
        bool WritingSeparation { get; set; }
    }
}
