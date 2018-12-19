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
    /// 迁移审计资源。
    /// </summary>
    public class MigrationAuditResource : Resources.IResource
    {
        /// <summary>
        /// 命令文本。
        /// </summary>
        public string CommandText { get; set; }

        /// <summary>
        /// 命令哈希。
        /// </summary>
        public string CommandHash { get; set; }
    }
}
