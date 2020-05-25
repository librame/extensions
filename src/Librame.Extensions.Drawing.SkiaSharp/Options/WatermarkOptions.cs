#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using SkiaSharp;
using System.Drawing;

namespace Librame.Extensions.Drawing.Options
{
    using Core.Combiners;
    using Core.Serializers;

    /// <summary>
    /// 水印选项。
    /// </summary>
    public class WatermarkOptions
    {
        /// <summary>
        /// 是否生成随机坐标（默认不随机）。
        /// </summary>
        public bool IsRandom { get; set; }
            = false;

        /// <summary>
        /// 水印文本。
        /// </summary>
        public string Text { get; set; }
            = "librame";

        /// <summary>
        /// 水印图片文件组合器。
        /// </summary>
        public FilePathCombiner ImagePath { get; set; }
            = new FilePathCombiner("watermark.png");

        /// <summary>
        /// 水印位置（负值表示反向）。
        /// </summary>
        public Point Location { get; set; }
            = new Point(30, 50);

        /// <summary>
        /// 颜色选项。
        /// </summary>
        public SKColorOptions Colors { get; }
            = new SKColorOptions
            {
                Fore = new SerializableString<SKColor>(SKColorThemes.WatermarkForeColor),
                Alternate = new SerializableString<SKColor>(SKColorThemes.WatermarkAlternateColor)
            };

        /// <summary>
        /// 字体。
        /// </summary>
        public FontOptions Font { get; }
            = new FontOptions
            {
                Size = 32
            };
    }
}
