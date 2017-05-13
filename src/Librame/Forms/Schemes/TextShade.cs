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

namespace Librame.Forms.Schemes
{
    /// <summary>
    /// 文本阴影。
    /// </summary>
    [Description("文本阴影")]
    public enum TextShade
    {
        /// <summary>
        /// 白色。
        /// </summary>
        [Description("白色")]
        White = 0xFFFFFF,

        /// <summary>
        /// 黑色。
        /// </summary>
        [Description("黑色")]
        Black = 0x212121
    }
}
