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
    /// 颜色主要方案。
    /// </summary>
    public class ColorPrimaryScheme : PrimaryScheme<Color>
    {
        /// <summary>
        /// 构造一个 <see cref="ColorPrimaryScheme"/> 实例。
        /// </summary>
        /// <param name="primary">给定的主颜色。</param>
        /// <param name="darkPrimary">给定的暗主颜色。</param>
        /// <param name="lightPrimary">给定的亮主颜色。</param>
        /// <param name="accent">给定的强调颜色。</param>
        /// <param name="textShade">给定的文本阴影。</param>
        public ColorPrimaryScheme(PrimaryColor primary, PrimaryColor darkPrimary, PrimaryColor lightPrimary, AccentColor accent, TextShade textShade)
            : base(ColorHelper.ToColor(primary), ColorHelper.ToColor(darkPrimary), ColorHelper.ToColor(lightPrimary), ColorHelper.ToColor(accent), ColorHelper.ToColor(textShade))
        {
        }
        /// <summary>
        /// 构造一个 <see cref="ColorPrimaryScheme"/> 实例。
        /// </summary>
        /// <param name="primary">给定的主颜色。</param>
        /// <param name="darkPrimary">给定的暗主颜色。</param>
        /// <param name="lightPrimary">给定的亮主颜色。</param>
        /// <param name="accent">给定的强调颜色。</param>
        /// <param name="textShade">给定的文本阴影颜色。</param>
        public ColorPrimaryScheme(Color primary, Color darkPrimary, Color lightPrimary, Color accent, Color textShade)
            : base(primary, darkPrimary, lightPrimary, accent, textShade)
        {
        }


        #region Defaults

        /// <summary>
        /// 获取红色方案。
        /// </summary>
        public static readonly ColorPrimaryScheme Red = new ColorPrimaryScheme(PrimaryColor.Red700,
            PrimaryColor.Red900,
            PrimaryColor.Red300,
            AccentColor.LightBlue400,
            Schemes.TextShade.White);

        /// <summary>
        /// 获取粉色方案。
        /// </summary>
        public static readonly ColorPrimaryScheme Pink = new ColorPrimaryScheme(PrimaryColor.Pink700,
            PrimaryColor.Pink900,
            PrimaryColor.Pink300,
            AccentColor.Blue400,
            Schemes.TextShade.Black);

        /// <summary>
        /// 获取紫色方案。
        /// </summary>
        public static readonly ColorPrimaryScheme Purple = new ColorPrimaryScheme(PrimaryColor.Purple700,
            PrimaryColor.Purple900,
            PrimaryColor.Purple300,
            AccentColor.LightBlue400,
            Schemes.TextShade.White);

        /// <summary>
        /// 获取深紫色方案。
        /// </summary>
        public static readonly ColorPrimaryScheme DeepPurple = new ColorPrimaryScheme(PrimaryColor.DeepPurple700,
            PrimaryColor.DeepPurple900,
            PrimaryColor.DeepPurple300,
            AccentColor.Cyan400,
            Schemes.TextShade.White);

        /// <summary>
        /// 获取靛蓝色方案。
        /// </summary>
        public static readonly ColorPrimaryScheme Indigo = new ColorPrimaryScheme(PrimaryColor.Indigo700,
            PrimaryColor.Indigo900,
            PrimaryColor.Indigo300,
            AccentColor.Teal400,
            Schemes.TextShade.White);

        /// <summary>
        /// 获取蓝色方案。
        /// </summary>
        public static readonly ColorPrimaryScheme Blue = new ColorPrimaryScheme(PrimaryColor.Blue700,
            PrimaryColor.Blue900,
            PrimaryColor.Blue300,
            AccentColor.Pink400,
            Schemes.TextShade.White);

        /// <summary>
        /// 获取亮蓝色方案。
        /// </summary>
        public static readonly ColorPrimaryScheme LightBlue = new ColorPrimaryScheme(PrimaryColor.LightBlue700,
            PrimaryColor.LightBlue900,
            PrimaryColor.LightBlue300,
            AccentColor.Purple400,
            Schemes.TextShade.Black);

        /// <summary>
        /// 获取青色方案。
        /// </summary>
        public static readonly ColorPrimaryScheme Cyan = new ColorPrimaryScheme(PrimaryColor.Cyan700,
            PrimaryColor.Cyan900,
            PrimaryColor.Cyan300,
            AccentColor.DeepPurple400,
            Schemes.TextShade.White);

        /// <summary>
        /// 获取水鸭色方案。
        /// </summary>
        public static readonly ColorPrimaryScheme Teal = new ColorPrimaryScheme(PrimaryColor.Teal700,
            PrimaryColor.Teal900,
            PrimaryColor.Teal300,
            AccentColor.Indigo400,
            Schemes.TextShade.White);

        /// <summary>
        /// 获取绿色方案。
        /// </summary>
        public static readonly ColorPrimaryScheme Green = new ColorPrimaryScheme(PrimaryColor.Green700,
            PrimaryColor.Green900,
            PrimaryColor.Green300,
            AccentColor.Amber400,
            Schemes.TextShade.White);

        /// <summary>
        /// 获取亮绿色方案。
        /// </summary>
        public static readonly ColorPrimaryScheme LightGreen = new ColorPrimaryScheme(PrimaryColor.LightGreen700,
            PrimaryColor.LightGreen900,
            PrimaryColor.LightGreen300,
            AccentColor.Orange400,
            Schemes.TextShade.Black);

        /// <summary>
        /// 获取绿黄色方案。
        /// </summary>
        public static readonly ColorPrimaryScheme Lime = new ColorPrimaryScheme(PrimaryColor.Lime700,
            PrimaryColor.Lime900,
            PrimaryColor.Lime300,
            AccentColor.DeepOrange400,
            Schemes.TextShade.White);

        /// <summary>
        /// 获取黄色方案。
        /// </summary>
        public static readonly ColorPrimaryScheme Yellow = new ColorPrimaryScheme(PrimaryColor.Yellow700,
            PrimaryColor.Yellow900,
            PrimaryColor.Yellow300,
            AccentColor.Purple400,
            Schemes.TextShade.Black);

        /// <summary>
        /// 获取琥珀色方案。
        /// </summary>
        public static readonly ColorPrimaryScheme Amber = new ColorPrimaryScheme(PrimaryColor.Amber700,
            PrimaryColor.Amber900,
            PrimaryColor.Amber300,
            AccentColor.Green400,
            Schemes.TextShade.White);

        /// <summary>
        /// 获取橙色方案。
        /// </summary>
        public static readonly ColorPrimaryScheme Orange = new ColorPrimaryScheme(PrimaryColor.Orange700,
            PrimaryColor.Orange900,
            PrimaryColor.Orange300,
            AccentColor.LightGreen400,
            Schemes.TextShade.White);

        /// <summary>
        /// 获取深橙色方案。
        /// </summary>
        public static readonly ColorPrimaryScheme DeepOrange = new ColorPrimaryScheme(PrimaryColor.DeepOrange700,
            PrimaryColor.DeepOrange900,
            PrimaryColor.DeepOrange300,
            AccentColor.Lime400,
            Schemes.TextShade.White);

        /// <summary>
        /// 获取棕色方案。
        /// </summary>
        public static readonly ColorPrimaryScheme Brown = new ColorPrimaryScheme(PrimaryColor.Brown700,
            PrimaryColor.Brown900,
            PrimaryColor.Brown300,
            AccentColor.Yellow400,
            Schemes.TextShade.White);

        /// <summary>
        /// 获取灰色方案。
        /// </summary>
        public static readonly ColorPrimaryScheme Grey = new ColorPrimaryScheme(PrimaryColor.Grey700,
            PrimaryColor.Grey900,
            PrimaryColor.Grey300,
            AccentColor.Purple400,
            Schemes.TextShade.Black);

        /// <summary>
        /// 获取蓝灰色方案。
        /// </summary>
        public static readonly ColorPrimaryScheme BlueGrey = new ColorPrimaryScheme(PrimaryColor.BlueGrey700,
            PrimaryColor.BlueGrey900,
            PrimaryColor.BlueGrey300,
            AccentColor.Red400,
            Schemes.TextShade.White);

        #endregion

    }
}
