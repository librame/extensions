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
        /// 名称。
        /// </summary>
        [Description("名称")]
        Name,

        /// <summary>
        /// 文本。
        /// </summary>
        [Description("文本")]
        Text,

        /// <summary>
        /// 尺寸。
        /// </summary>
        [Description("尺寸")]
        Measure,

        /// <summary>
        /// 选项。
        /// </summary>
        [Description("选项")]
        Options,

        /// <summary>
        /// 名称文本。
        /// </summary>
        [Description("名称文本")]
        NameText,

        /// <summary>
        /// 尺寸文本。
        /// </summary>
        [Description("尺寸文本")]
        MeasureText,

        /// <summary>
        /// 信息。
        /// </summary>
        [Description("信息")]
        Info,

        /// <summary>
        /// 基本知识。
        /// </summary>
        [Description("基本知识")]
        HowTo
    }
}
