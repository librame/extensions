#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.ComponentModel;

namespace Librame.Extensions.Storage.Capacities
{
    /// <summary>
    /// 单位格式。
    /// </summary>
    [Description("单位格式")]
    public enum UnitFormat
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
        KByte,

        /// <summary>
        /// 兆字节。
        /// </summary>
        [Description("兆字节")]
        MByte,

        /// <summary>
        /// 吉字节。
        /// </summary>
        [Description("吉字节")]
        GByte,

        /// <summary>
        /// 太字节。
        /// </summary>
        [Description("太字节")]
        TByte,

        /// <summary>
        /// 拍字节。
        /// </summary>
        [Description("拍字节")]
        PByte,

        /// <summary>
        /// 艾字节。
        /// </summary>
        [Description("艾字节")]
        EByte,

        /// <summary>
        /// 泽字节。
        /// </summary>
        [Description("泽字节")]
        ZByte,

        /// <summary>
        /// 尧字节。
        /// </summary>
        [Description("尧字节")]
        YByte
    }
}
