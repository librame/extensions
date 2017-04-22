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

namespace Librame.Forms
{
    /// <summary>
    /// 鼠标状态。
    /// </summary>
    [Description("鼠标状态")]
    public enum MouseState
    {
        /// <summary>
        /// 默认。
        /// </summary>
        [Description("默认")]
        Default = 0,

        /// <summary>
        /// 悬停。
        /// </summary>
        [Description("悬停")]
        Hover = 1,

        /// <summary>
        /// 按下。
        /// </summary>
        [Description("按下")]
        Down = 2,

        /// <summary>
        /// 移出。
        /// </summary>
        [Description("移出")]
        Out = 4
    }
}
