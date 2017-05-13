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

namespace Librame.MediaInfo
{
    /// <summary>
    /// 信息文件选项。
    /// </summary>
    [Description("信息文件选项")]
    public enum InfoFileOptions
    {
        /// <summary>
        /// Nothing。
        /// </summary>
        FileOption_Nothing = 0x00,

        /// <summary>
        /// NoRecursive。
        /// </summary>
        FileOption_NoRecursive = 0x01,

        /// <summary>
        /// CloseAll。
        /// </summary>
        FileOption_CloseAll = 0x02,

        /// <summary>
        /// Max。
        /// </summary>
        FileOption_Max = 0x04
    }
}
