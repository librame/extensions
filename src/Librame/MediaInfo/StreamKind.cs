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
    /// 流种类。
    /// </summary>
    [Description("流种类")]
    public enum StreamKind
    {
        /// <summary>
        /// 概要。
        /// </summary>
        [Description("概要")]
        General,

        /// <summary>
        /// 视频。
        /// </summary>
        [Description("视频")]
        Video,

        /// <summary>
        /// 音频。
        /// </summary>
        [Description("音频")]
        Audio,

        /// <summary>
        /// 字幕。
        /// </summary>
        [Description("字幕")]
        Text,

        /// <summary>
        /// 其它。
        /// </summary>
        [Description("其它")]
        Other,

        /// <summary>
        /// 图像。
        /// </summary>
        [Description("图像")]
        Image,

        /// <summary>
        /// 菜单。
        /// </summary>
        [Description("菜单")]
        Menu
    }
}
