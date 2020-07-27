#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Options
{
    /// <summary>
    /// 数据存储选项。
    /// </summary>
    public class DataStoreOptions : AbstractStoreOptions
    {
        /// <summary>
        /// 使用数据审计（默认已启用）。
        /// </summary>
        public bool UseDataAudit { get; set; }
            = true;

        /// <summary>
        /// 使用数据迁移（默认已启用，禁用可能会导致部分迁移功能失效）。
        /// </summary>
        public bool UseDataMigration { get; set; }
            = true;

        /// <summary>
        /// 使用数据表格（默认已启用，禁用可能会导致部分迁移功能失效）。
        /// </summary>
        public bool UseDataTabulation { get; set; }
            = true;

        /// <summary>
        /// 使用数据租户（默认已启用，禁用可能会影响多租户功能）。
        /// </summary>
        public bool UseDataTenant { get; set; }
            = true;
    }
}
