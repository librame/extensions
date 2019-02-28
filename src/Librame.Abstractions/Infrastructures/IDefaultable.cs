#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace System
{
    /// <summary>
    /// 可默认接口。
    /// </summary>
    public interface IDefaultable
    {
        /// <summary>
        /// 是否为默认值。
        /// </summary>
        /// <returns>返回布尔值。</returns>
        bool IsDefaulting();
    }
}
