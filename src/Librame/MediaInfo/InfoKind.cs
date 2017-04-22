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
    /// 信息种类。
    /// </summary>
    [Description("信息种类")]
    public enum InfoKind
    {
        /// <summary>
        /// 默认。
        /// </summary>
        [Description("默认")]
        Default = 0,

        /// <summary>
        /// 名称。
        /// </summary>
        [Description("名称")]
        Name = 1,

        /// <summary>
        /// 文本。
        /// </summary>
        [Description("文本")]
        Text = 2,

        /// <summary>
        /// 尺寸。
        /// </summary>
        [Description("尺寸")]
        Measure = 4,

        /// <summary>
        /// 选项。
        /// </summary>
        [Description("选项")]
        Options = 8,

        /// <summary>
        /// 名称文本。
        /// </summary>
        [Description("名称文本")]
        NameText = 16,

        /// <summary>
        /// 尺寸文本。
        /// </summary>
        [Description("尺寸文本")]
        MeasureText = 32,

        /// <summary>
        /// 信息。
        /// </summary>
        [Description("信息")]
        Info = 64,

        /// <summary>
        /// 基本知识。
        /// </summary>
        [Description("基本知识")]
        HowTo = 128
    }
}
