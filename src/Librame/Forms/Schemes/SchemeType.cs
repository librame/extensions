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
    /// 方案类型。
    /// </summary>
    public enum SchemeType
    {
        /// <summary>
        /// 白色（亮系）。
        /// </summary>
        [Description("白色（亮系）")]
        WhiteOrLight,

        /// <summary>
        /// 黑色（暗系）。
        /// </summary>
        [Description("黑色（暗系）")]
        BlackOrDark
    }
}
