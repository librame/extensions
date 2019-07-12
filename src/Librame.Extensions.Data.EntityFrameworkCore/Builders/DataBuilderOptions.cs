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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 数据构建器选项。
    /// </summary>
    public class DataBuilderOptions : AbstractDataBuilderOptions<StoreOptions, TableSchemaOptions>
    {
        /// <summary>
        /// 默认架构。
        /// </summary>
        public string DefaultSchema { get; set; }

        /// <summary>
        /// 启用审计（默认已启用）。
        /// </summary>
        public bool EnableAudit { get; set; }
            = true;

        /// <summary>
        /// 启用租户（默认已启用）。
        /// </summary>
        public bool EnableTenant { get; set; }
            = true;

        /// <summary>
        /// 是否创建数据库（如果数据库不存在；默认已启用）。
        /// </summary>
        public bool IsCreateDatabase { get; set; }
            = true;

        /// <summary>
        /// 已创建数据库动作。
        /// </summary>
        public Action<IAccessor> DatabaseCreatedAction { get; set; }

        /// <summary>
        /// 默认租户。
        /// </summary>
        public ITenant DefaultTenant { get; set; }
            = new Tenant();
    }
}
