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

namespace Librame.Forms
{
    /// <summary>
    /// <see cref="Color"/> 助手。
    /// </summary>
    public class ColorHelper
    {
        /// <summary>
        /// 将强调颜色转换为颜色。
        /// </summary>
        /// <param name="accent">给定的强调颜色。</param>
        /// <returns>返回 <see cref="Color"/>。</returns>
        public static Color ToColor(Schemes.AccentColor accent)
        {
            return ToColor((int)accent);
        }

        /// <summary>
        /// 将主颜色转换为颜色。
        /// </summary>
        /// <param name="primary">给定的主颜色。</param>
        /// <returns>返回 <see cref="Color"/>。</returns>
        public static Color ToColor(Schemes.PrimaryColor primary)
        {
            return ToColor((int)primary);
        }

        /// <summary>
        /// 将文本阴影转换为颜色。
        /// </summary>
        /// <param name="textShade">给定的文本阴影。</param>
        /// <returns>返回 <see cref="Color"/>。</returns>
        public static Color ToColor(Schemes.TextShade textShade)
        {
            return ToColor((int)textShade);
        }

        /// <summary>
        /// 将整数转换为颜色。
        /// </summary>
        /// <param name="argb">给定的整数值。</param>
        /// <returns>返回 <see cref="Color"/>。</returns>
        public static Color ToColor(int argb)
        {
            return Color.FromArgb(
                (argb & 0xff0000) >> 16,
                (argb & 0xff00) >> 8,
                 argb & 0xff);
        }


        /// <summary>
        /// 移除颜色的透明通道。
        /// </summary>
        /// <param name="color">给定的 <see cref="Color"/>。</param>
        /// <returns>返回 <see cref="Color"/>。</returns>
        public static Color RemoveAlpha(Color color)
        {
            return Color.FromArgb(color.R, color.G, color.B);
        }


        /// <summary>
        /// 将 0~100 的百分比整数值转换为 0~255 的颜色值。
        /// </summary>
        /// <param name="percentage">给定的百分比整数值。</param>
        /// <returns>返回整数值。</returns>
        public static int PercentageToColorComponent(int percentage)
        {
            return (int)((percentage / 100d) * 255d);
        }

    }
}
