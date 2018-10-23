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
using System.Collections.Generic;

namespace Librame.Extensions.Data
{
    using Builders;

    /// <summary>
    /// 数据构建器选项接口。
    /// </summary>
    public interface IDataBuilderOptions : IBuilderOptions
    {
        /// <summary>
        /// 默认架构。
        /// </summary>
        string DefaultSchema { get; set; }

        /// <summary>
        /// 启用审计（默认已启用）。
        /// </summary>
        bool AuditEnabled { get; set; }

        /// <summary>
        /// 确保数据库已创建（默认已启用）。
        /// </summary>
        bool EnsureDbCreated { get; set; }

        /// <summary>
        /// 发布审计事件动作方法。
        /// </summary>
        Action<IDbContext, IList<Audit>> PublishAuditEvent { get; set; }


        /// <summary>
        /// 连接选项。
        /// </summary>
        ConnectionOptions Connection { get; set; }

        /// <summary>
        /// 审计表选项。
        /// </summary>
        ITableOptions AuditTable { get; set; }

        /// <summary>
        /// 审计属性表选项。
        /// </summary>
        IShardingOptions AuditPropertyTable { get; set; }
    }
}
