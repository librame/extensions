#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Drawing;

namespace Librame.Extensions.Drawing.Options
{
    /// <summary>
    /// 缩放选项。
    /// </summary>
    public class ScaleOptions
    {
        /// <summary>
        /// 文件名后缀。
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// 水印模式。
        /// </summary>
        public WatermarkMode Watermark { get; set; }
            = WatermarkMode.None;

        /// <summary>
        /// 最大尺寸。
        /// </summary>
        public Size MaxSize { get; set; }
            = new Size(100, 70);
    }
}
