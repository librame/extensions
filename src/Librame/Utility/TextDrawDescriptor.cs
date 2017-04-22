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

namespace Librame.Utility
{
    /// <summary>
    /// 文本绘制描述符。
    /// </summary>
    public class TextDrawDescriptor
    {
        /// <summary>
        /// 默认文本绘制描述符。
        /// </summary>
        public readonly static TextDrawDescriptor Default = new TextDrawDescriptor()
        {
            Font = new Font(FontFamily.GenericSansSerif, 14),
            BgColor = Color.WhiteSmoke,
            ForeColor = Color.DarkGray,
            LineColor = Color.Blue,
            ShadowColor = Color.Black
        };


        /// <summary>
        /// 获取或设置字体。
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// 获取或设置背景色。
        /// </summary>
        public Color BgColor { get; set; }

        /// <summary>
        /// 获取或设置前景色。
        /// </summary>
        public Color ForeColor { get; set; }

        /// <summary>
        /// 获取或设置线条色。
        /// </summary>
        public Color LineColor { get; set; }

        /// <summary>
        /// 获取或设置阴影色。
        /// </summary>
        public Color ShadowColor { get; set; }
    }
}
