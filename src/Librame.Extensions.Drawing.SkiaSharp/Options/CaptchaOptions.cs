#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Drawing.Options
{
    /// <summary>
    /// 验证码选项。
    /// </summary>
    public class CaptchaOptions
    {
        /// <summary>
        /// 颜色。
        /// </summary>
        public SKColorOptions Colors { get; }
            = new SKColorOptions();

        /// <summary>
        /// 字体。
        /// </summary>
        public FontOptions Font { get; }
            = new FontOptions { Size = 16 };

        /// <summary>
        /// 噪点。
        /// </summary>
        public BackgroundNoiseOptions Noise { get; }
            = new BackgroundNoiseOptions();
    }
}
