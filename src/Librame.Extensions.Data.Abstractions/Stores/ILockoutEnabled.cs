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
    /// 启用锁定。
    /// </summary>
    /// <typeparam name="TDateTime">指定的日期与时间类型。</typeparam>
    public interface ILockoutEnabled<TDateTime>
        where TDateTime : struct
    {
        /// <summary>
        /// 启用锁定。
        /// </summary>
        bool LockoutEnabled { get; set; }

        /// <summary>
        /// 锁定结束时间。
        /// </summary>
        TDateTime LockoutEndTime { get; set; }
    }
}
