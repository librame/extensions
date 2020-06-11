#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Resources
{
    using Core.Resources;

    /// <summary>
    /// 租户资源。
    /// </summary>
    public class DataTenantResource : IResource
    {
        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 主机。
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 默认连接字符串。
        /// </summary>
        public string DefaultConnectionString { get; set; }

        /// <summary>
        /// 写入连接字符串。
        /// </summary>
        public string WriteConnectionString { get; set; }

        /// <summary>
        /// 写入分离。
        /// </summary>
        public string WriteSeparation { get; set; }

        /// <summary>
        /// 加密连接字符串集合。
        /// </summary>
        public string EncryptedConnectionStrings { get; set; }

        /// <summary>
        /// 数据同步。
        /// </summary>
        public string DataSynchronization { get; set; }

        /// <summary>
        /// 结构同步。
        /// </summary>
        public string StructureSynchronization { get; set; }
    }
}
