#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 锁定期。
    /// </summary>
    /// <typeparam name="TLockoutTime">指定的锁定期时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
    public interface ILockout<TLockoutTime> : IObjectLockout
        where TLockoutTime : struct
    {
        /// <summary>
        /// 锁定期结束时间。
        /// </summary>
        TLockoutTime? LockoutEnd { get; set; }

        /// <summary>
        /// 已锁定。
        /// </summary>
        bool LockoutEnabled { get; set; }
    }
}
