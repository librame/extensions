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
    /// 存储选项接口。
    /// </summary>
    public interface IStoreOptions
    {
        /// <summary>
        /// 映射关系。
        /// </summary>
        bool MapRelationship { get; set; }

        /// <summary>
        /// 属性的最大长度。
        /// </summary>
        int MaxLengthForProperties { get; set; }
    }
}
