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
        /// 默认。
        /// </summary>
        [Description("默认")]
        Default = 0,

        /// <summary>
        /// 概要。
        /// </summary>
        [Description("概要")]
        General = 1,

        /// <summary>
        /// 视频。
        /// </summary>
        [Description("视频")]
        Video = 2,

        /// <summary>
        /// 音频。
        /// </summary>
        [Description("音频")]
        Audio = 4,

        /// <summary>
        /// 字幕。
        /// </summary>
        [Description("字幕")]
        Text = 8,

        /// <summary>
        /// 章节。
        /// </summary>
        [Description("章节")]
        Chapters = 16,

        /// <summary>
        /// 图像。
        /// </summary>
        [Description("图像")]
        Image = 32
    }
}
