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
    /// 数据租户资源。
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
        public string WriteConnectionSeparation { get; set; }
    }
}
