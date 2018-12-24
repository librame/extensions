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
        /// 快照名称。
        /// </summary>
        public string SnapshotName { get; set; }

        /// <summary>
        /// 快照代码。
        /// </summary>
        public string SnapshotCode { get; set; }

        /// <summary>
        /// 快照哈希。
        /// </summary>
        public string SnapshotHash { get; set; }
    }
}
