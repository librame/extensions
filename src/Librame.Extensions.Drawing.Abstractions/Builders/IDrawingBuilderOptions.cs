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

namespace Librame.Extensions.Drawing
{
    using Builders;

    /// <summary>
    /// 图画构建器选项接口。
    /// </summary>
    public interface IDrawingBuilderOptions : IBuilderOptions
    {
        /// <summary>
        /// 图像格式（默认为 JPEG）。
        /// </summary>
        ImageFormat ImageFormat { get; set; }

        /// <summary>
        /// 图像品质（取值范围：1-100；默认为 80）。
        /// </summary>
        int Quality { get; set; }

        /// <summary>
        /// 图像文件扩展名集合（以英文逗号分隔）。
        /// </summary>
        string ImageExtensions { get; set; }

        /// <summary>
        /// 缩放选项。
        /// </summary>
        IList<ScaleOptions> Scales { get; set; }

        /// <summary>
        /// 验证码选项。
        /// </summary>
        CaptchaOptions Captcha { get; set; }

        /// <summary>
        /// 水印选项。
        /// </summary>
        WatermarkOptions Watermark { get; set; }
    }
}
