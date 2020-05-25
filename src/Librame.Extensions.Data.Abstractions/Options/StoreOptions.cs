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
    using Stores;

    /// <summary>
    /// 存储选项。
    /// </summary>
    public class StoreOptions
    {
        /// <summary>
        /// 映射关系（默认不启用）。
        /// </summary>
        public bool MapRelationship { get; set; }
            = false;

        /// <summary>
        /// 属性集合的最大长度（默认为 250）。
        /// </summary>
        public int MaxLengthForProperties { get; set; }
            = 250;

        /// <summary>
        /// 保护隐私数据（默认已启用）。
        /// </summary>
        public bool ProtectPrivacyData { get; set; }
            = true;

        /// <summary>
        /// 使用数据审计（默认已启用）。
        /// </summary>
        public bool UseDataAudit { get; set; }
            = true;

        /// <summary>
        /// 使用数据实体（默认已启用）。
        /// </summary>
        public bool UseDataEntity { get; set; }
            = true;

        /// <summary>
        /// 使用数据迁移（默认已启用）。
        /// </summary>
        public bool UseDataMigration { get; set; }
            = true;

        /// <summary>
        /// 使用数据租户（默认已启用）。
        /// </summary>
        public bool UseDataTenant { get; set; }
            = true;

        /// <summary>
        /// 使用 <see cref="IStoreInitializer{TGenId}"/> 进行数据初始化（默认已启用）。
        /// </summary>
        public bool UseInitializer { get; set; }
            = true;
    }
}
