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
    /// 启用锁定。
    /// </summary>
    public interface ILockoutEnabled
    {
        /// <summary>
        /// 启用锁定。
        /// </summary>
        bool LockoutEnabled { get; set; }

        /// <summary>
        /// 锁定结束时间。
        /// </summary>
        DateTimeOffset? LockoutEndTime { get; set; }
    }
}
