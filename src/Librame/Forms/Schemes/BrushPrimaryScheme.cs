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
    /// 笔刷主要方案。
    /// </summary>
    public class BrushPrimaryScheme : PrimaryScheme<Brush>
    {
        /// <summary>
        /// 构造一个 <see cref="BrushPrimaryScheme"/> 实例。
        /// </summary>
        /// <param name="primaryColors">给定的主要颜色方案。</param>
        public BrushPrimaryScheme(ColorPrimaryScheme primaryColors)
            : base(new SolidBrush(primaryColors.Primary),
                  new SolidBrush(primaryColors.DarkPrimary),
                  new SolidBrush(primaryColors.LightPrimary),
                  new SolidBrush(primaryColors.Accent),
                  new SolidBrush(primaryColors.TextShade))
        {
        }
        /// <summary>
        /// 构造一个 <see cref="BrushPrimaryScheme"/> 实例。
        /// </summary>
        /// <param name="primary">给定的主笔刷。</param>
        /// <param name="darkPrimary">给定的暗主笔刷。</param>
        /// <param name="lightPrimary">给定的亮主笔刷。</param>
        /// <param name="accent">给定的强调笔刷。</param>
        /// <param name="textShade">给定的文本阴影笔刷。</param>
        public BrushPrimaryScheme(Brush primary, Brush darkPrimary, Brush lightPrimary, Brush accent, Brush textShade)
            : base(primary, darkPrimary, lightPrimary, accent, textShade)
        {
        }

    }
}
