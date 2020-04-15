#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using SkiaSharp;
using System.Collections.Generic;
using System.Drawing;

namespace Librame.Extensions.Drawing.Builders
{
    using Core.Builders;
    using Core.Serializers;

    /// <summary>
    /// 图画构建器选项。
    /// </summary>
    public class DrawingBuilderOptions : IExtensionBuilderOptions
    {
        /// <summary>
        /// 图像格式（默认为 JPEG）。
        /// </summary>
        public ImageFormat ImageFormat { get; set; }
            = ImageFormat.Jpeg;

        /// <summary>
        /// 图像品质（取值范围：1-100；默认为 80）。
        /// </summary>
        public int Quality { get; set; }
            = 80;

        /// <summary>
        /// 图像文件扩展名集合（以英文逗号分隔）。
        /// </summary>
        public string ImageExtensions { get; set; }
            = ".bmp,.heif,.heic,.jpg,.jpeg,.jfif,.png,.webp";

        /// <summary>
        /// 验证码选项。
        /// </summary>
        public CaptchaOptions Captcha { get; }
            = new CaptchaOptions();

        /// <summary>
        /// 水印选项。
        /// </summary>
        public WatermarkOptions Watermark { get; }
            = new WatermarkOptions();

        /// <summary>
        /// 缩放选项。
        /// </summary>
        public IReadOnlyList<ScaleOptions> Scales { get; set; }
            = new List<ScaleOptions>
            {
                new ScaleOptions
                {
                    Suffix = "-small",
                    Watermark = WatermarkMode.None,
                    MaxSize = new Size(100, 70)
                },
                new ScaleOptions
                {
                    Suffix = "-large",
                    Watermark = WatermarkMode.Text,
                    MaxSize = new Size(1000, 700)
                }
            };
    }


    /// <summary>
    /// 验证码选项。
    /// </summary>
    public class CaptchaOptions
    {
        /// <summary>
        /// 颜色。
        /// </summary>
        public ColorOptions Colors { get; }
            = new ColorOptions();

        /// <summary>
        /// 字体。
        /// </summary>
        public FontOptions Font { get; }
            = new FontOptions { Size = 16 };

        /// <summary>
        /// 噪点。
        /// </summary>
        public NoiseOptions Noise { get; }
            = new NoiseOptions();
    }


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
        public string ImagePath { get; set; }
            = "watermark.png";

        /// <summary>
        /// 水印位置（负值表示反向）。
        /// </summary>
        public Point Location { get; set; }
            = new Point(30, 50);

        /// <summary>
        /// 颜色选项。
        /// </summary>
        public ColorOptions Colors { get; }
            = new ColorOptions
            {
                Fore = new SerializableString<SKColor>(SKColorSettings.WatermarkForeColor),
                Alternate = new SerializableString<SKColor>(SKColorSettings.WatermarkAlternateColor)
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


    /// <summary>
    /// 颜色选项。
    /// </summary>
    public class ColorOptions
    {
        /// <summary>
        /// 前景色。
        /// </summary>
        public SerializableString<SKColor> Fore { get; set; }
            = new SerializableString<SKColor>(SKColorSettings.ForeColor);

        /// <summary>
        /// 交替色。
        /// </summary>
        public SerializableString<SKColor> Alternate { get; set; }
            = new SerializableString<SKColor>(SKColorSettings.ForeColor);

        /// <summary>
        /// 背景色。
        /// </summary>
        public SerializableString<SKColor> Background { get; set; }
            = new SerializableString<SKColor>(SKColorSettings.ForeColor);

        /// <summary>
        /// 干扰色。
        /// </summary>
        public SerializableString<SKColor> Disturbing { get; set; }
            = new SerializableString<SKColor>(SKColorSettings.ForeColor);
    }


    /// <summary>
    /// 字体选项。
    /// </summary>
    public class FontOptions
    {
        /// <summary>
        /// 字体文件组合器。
        /// </summary>
        public string FilePath { get; set; }
            = "font.ttf";

        /// <summary>
        /// 大小。
        /// </summary>
        public int Size { get; set; }
            = 16;
    }


    /// <summary>
    /// 背景噪点选项。
    /// </summary>
    public class NoiseOptions
    {
        /// <summary>
        /// 宽度。
        /// </summary>
        public int Width { get; set; }
            = 2;

        /// <summary>
        /// 间距。
        /// </summary>
        public PointF Space { get; set; }
            = new PointF
            {
                X = 5,
                Y = 5
            };
    }

}
