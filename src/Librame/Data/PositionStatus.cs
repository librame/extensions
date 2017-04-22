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

namespace Librame.Data
{
    /// <summary>
    /// 定位状态。
    /// </summary>
    [Description("定位状态")]
    public enum PositionStatus
    {
        /// <summary>
        /// 默认。
        /// </summary>
        [Description("默认")]
        Default = 0,

        /// <summary>
        /// 所有。
        /// </summary>
        [Description("所有")]
        All = 1,

        /// <summary>
        /// 导航栏。
        /// </summary>
        [Description("导航栏")]
        Navbar = 2,

        /// <summary>
        /// 定位栏。
        /// </summary>
        [Description("定位栏")]
        Locabar = 4,

        /// <summary>
        /// 侧边栏。
        /// </summary>
        [Description("侧边栏")]
        Sidebar = 8,

        /// <summary>
        /// 工具栏。
        /// </summary>
        [Description("工具栏")]
        Toolbar = 16,

        /// <summary>
        /// 底栏。
        /// </summary>
        [Description("底栏")]
        Footbar = 32
    }
}
