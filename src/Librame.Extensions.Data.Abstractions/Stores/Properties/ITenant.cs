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
        /// 写入分离（如果启用，所有数据将保存到写入连接）。
        /// </summary>
        bool WritingSeparation { get; set; }

        /// <summary>
        /// 数据同步（如果启用，默认与写入连接的数据互为镜像；此功能已禁用）。
        /// </summary>
        bool DataSynchronization { get; set; }

        /// <summary>
        /// 结构同步（如果启用，默认与写入连接的结构互为镜像）。
        /// </summary>
        bool StructureSynchronization { get; set; }
    }
}
