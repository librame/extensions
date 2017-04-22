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

namespace Librame.Forms.Schemes
{
    /// <summary>
    /// 扁平按钮方案。
    /// </summary>
    public class FlatButtonScheme
    {
        /// <summary>
        /// 构造一个 <see cref="FlatButtonScheme"/> 实例。
        /// </summary>
        /// <param name="disabledTextColor">给定的禁用文本颜色。</param>
        /// <param name="backgroundHoverColor">给定的背景悬停颜色。</param>
        /// <param name="backgroundPressedColor">给定的背景按下颜色。</param>
        public FlatButtonScheme(Color disabledTextColor, Color backgroundHoverColor, Color backgroundPressedColor)
        {
            DisabledText = new SchemeInfo(disabledTextColor);
            BackgroundHover = new SchemeInfo(backgroundHoverColor);
            BackgroundPressed = new SchemeInfo(backgroundPressedColor);
        }


        /// <summary>
        /// 获取禁用文本方案。
        /// </summary>
        public SchemeInfo DisabledText { get; }

        /// <summary>
        /// 获取背景悬停方案。
        /// </summary>
        public SchemeInfo BackgroundHover { get; }

        /// <summary>
        /// 获取背景按下方案。
        /// </summary>
        public SchemeInfo BackgroundPressed { get; }


        #region Defaults

        /// <summary>
        /// 解析方案。
        /// </summary>
        /// <param name="schemeType">给定的方案类型。</param>
        /// <returns>返回 <see cref="FlatButtonScheme"/>。</returns>
        public static FlatButtonScheme Parse(SchemeType schemeType)
        {
            return (schemeType == SchemeType.BlackOrDark ? Dark : Light);
        }


        /// <summary>
        /// 获取暗系方案。
        /// </summary>
        public static readonly FlatButtonScheme Dark = new FlatButtonScheme(Color.FromArgb(ColorHelper.PercentageToColorComponent(30), ColorHelper.ToColor(0xFFFFFF)),
            Color.FromArgb(ColorHelper.PercentageToColorComponent(15), ColorHelper.ToColor(0xCCCCCC)),
            Color.FromArgb(ColorHelper.PercentageToColorComponent(25), ColorHelper.ToColor(0xCCCCCC)));

        /// <summary>
        /// 获取亮系方案。
        /// </summary>
        public static readonly FlatButtonScheme Light = new FlatButtonScheme(Color.FromArgb(ColorHelper.PercentageToColorComponent(26), ColorHelper.ToColor(0x000000)),
            Color.FromArgb(ColorHelper.PercentageToColorComponent(20), ColorHelper.ToColor(0x999999)),
            Color.FromArgb(ColorHelper.PercentageToColorComponent(40), ColorHelper.ToColor(0x999999)));

        #endregion

    }
}
