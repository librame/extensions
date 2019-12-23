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
    /// 启用锁定。
    /// </summary>
    /// <typeparam name="TLockoutEndTime">指定的锁定结束时间类型。</typeparam>
    public interface ILockoutEnabled<TLockoutEndTime>
        where TLockoutEndTime : struct
    {
        /// <summary>
        /// 启用锁定。
        /// </summary>
        bool LockoutEnabled { get; set; }

        /// <summary>
        /// 锁定结束时间。
        /// </summary>
        TLockoutEndTime? LockoutEndTime { get; set; }
    }
}
