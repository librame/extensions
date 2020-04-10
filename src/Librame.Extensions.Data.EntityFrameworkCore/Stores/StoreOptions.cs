#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 存储选项。
    /// </summary>
    public class StoreOptions
    {
        /// <summary>
        /// 启用初始化（默认已启用）。
        /// </summary>
        public bool InitializationEnabled { get; set; }
            = true;

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
    }
}
