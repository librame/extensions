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
    /// 数据迁移资源。
    /// </summary>
    public class DataMigrationResource : IResource
    {
        /// <summary>
        /// 访问器类型名。
        /// </summary>
        public virtual string AccessorName { get; set; }

        /// <summary>
        /// 模型快照类型名。
        /// </summary>
        public virtual string ModelSnapshotName { get; set; }

        /// <summary>
        /// 模型散列。
        /// </summary>
        public virtual string ModelHash { get; set; }

        /// <summary>
        /// 模型主体。
        /// </summary>
        public virtual string ModelBody { get; set; }
    }
}
