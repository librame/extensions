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

namespace Librame.Utility
{
    /// <summary>
    /// 文件大小单位。
    /// </summary>
    [Description("文件大小单位")]
    public enum FileSizeUnit : long
    {
        /// <summary>
        /// 字节。
        /// </summary>
        [Description("字节")]
        Byte = 0,

        /// <summary>
        /// 千字节。
        /// </summary>
        [Description("千字节")]
        KiB = 1024,

        /// <summary>
        /// 兆字节。
        /// </summary>
        [Description("兆字节")]
        MiB = 1048576,

        /// <summary>
        /// 吉字节。
        /// </summary>
        [Description("吉字节")]
        GiB = 1073741824,

        /// <summary>
        /// 太字节。
        /// </summary>
        [Description("太字节")]
        TiB = 1099511627776,

        /// <summary>
        /// 最大(拍)字节。
        /// </summary>
        [Description("最大(拍)字节")]
        Max = 1125899906842624
    }
}
