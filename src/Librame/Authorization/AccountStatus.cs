#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.ComponentModel;

namespace Librame.Authorization
{
    /// <summary>
    /// 帐户状态。
    /// </summary>
    [Description("帐户状态")]
    public enum AccountStatus
    {
        /// <summary>
        /// 默认。
        /// </summary>
        [Description("默认")]
        Default = 0,

        /// <summary>
        /// 活跃的。
        /// </summary>
        [Description("活跃的")]
        Active = 1,

        /// <summary>
        /// 被禁的。
        /// </summary>
        [Description("被禁的")]
        Banned = 2,

        /// <summary>
        /// 闲置的。
        /// </summary>
        [Description("闲置的")]
        Inactive = 4,

        /// <summary>
        /// 挂起的。
        /// </summary>
        [Description("挂起的")]
        Pending = 8
    }
}
