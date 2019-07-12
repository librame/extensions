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
    /// 存储选项。
    /// </summary>
    public class StoreOptions : IStoreOptions
    {
        /// <summary>
        /// 属性的最大长度。
        /// </summary>
        public int MaxLengthForProperties { get; set; }
    }
}
