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

namespace Librame.Extensions.Drawing
{
    internal static class SKColorThemes
    {
        /// <summary>
        /// 前景色。
        /// </summary>
        public readonly static SKColor ForeColor
            = SKColor.Parse("#0066cc");

        /// <summary>
        /// 交替色。
        /// </summary>
        public readonly static SKColor AlternateColor
            = SKColor.Parse("#993366");

        /// <summary>
        /// 背景色。
        /// </summary>
        public readonly static SKColor BackgroundColor
            = SKColor.Parse("#ccffff");

        /// <summary>
        /// 干扰色。
        /// </summary>
        public readonly static SKColor DisturbingColor
            = SKColor.Parse("#99ccff");

        /// <summary>
        /// 水印前景色。
        /// </summary>
        public readonly static SKColor WatermarkForeColor
            = SKColor.Parse("#ffffff");

        /// <summary>
        /// 水印交替色。
        /// </summary>
        public readonly static SKColor WatermarkAlternateColor
            = SKColor.Parse("#ccffff");
    }
}
