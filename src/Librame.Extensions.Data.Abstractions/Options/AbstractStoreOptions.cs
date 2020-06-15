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
    /// 抽象存储选项。
    /// </summary>
    public abstract class AbstractStoreOptions
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
    }
}
