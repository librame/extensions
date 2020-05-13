#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
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
}
