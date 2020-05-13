#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Drawing;

namespace Librame.Extensions.Drawing.Builders
{
    using Core.Builders;
    using Drawing.Options;

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
}
