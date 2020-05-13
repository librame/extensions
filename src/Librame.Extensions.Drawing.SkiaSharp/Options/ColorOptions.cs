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

namespace Librame.Extensions.Drawing.Options
{
    using Core.Serializers;

    /// <summary>
    /// 颜色选项。
    /// </summary>
    public class ColorOptions
    {
        /// <summary>
        /// 前景色。
        /// </summary>
        public SerializableString<SKColor> Fore { get; set; }
            = new SerializableString<SKColor>(SKColorThemes.ForeColor);

        /// <summary>
        /// 交替色。
        /// </summary>
        public SerializableString<SKColor> Alternate { get; set; }
            = new SerializableString<SKColor>(SKColorThemes.ForeColor);

        /// <summary>
        /// 背景色。
        /// </summary>
        public SerializableString<SKColor> Background { get; set; }
            = new SerializableString<SKColor>(SKColorThemes.ForeColor);

        /// <summary>
        /// 干扰色。
        /// </summary>
        public SerializableString<SKColor> Disturbing { get; set; }
            = new SerializableString<SKColor>(SKColorThemes.ForeColor);
    }
}
