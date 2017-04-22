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
    /// 线条主要方案。
    /// </summary>
    public class PenPrimaryScheme : PrimaryScheme<Pen>
    {
        /// <summary>
        /// 构造一个 <see cref="PenPrimaryScheme"/> 实例。
        /// </summary>
        /// <param name="primaryColors">给定的主要颜色方案。</param>
        public PenPrimaryScheme(ColorPrimaryScheme primaryColors)
            : base(new Pen(primaryColors.Primary),
                  new Pen(primaryColors.DarkPrimary),
                  new Pen(primaryColors.LightPrimary),
                  new Pen(primaryColors.Accent),
                  new Pen(primaryColors.TextShade))
        {
        }
        /// <summary>
        /// 构造一个 <see cref="PenPrimaryScheme"/> 实例。
        /// </summary>
        /// <param name="primary">给定的主线条。</param>
        /// <param name="darkPrimary">给定的暗主线条。</param>
        /// <param name="lightPrimary">给定的亮主线条。</param>
        /// <param name="accent">给定的强调线条。</param>
        /// <param name="textShade">给定的文本阴影。</param>
        public PenPrimaryScheme(Pen primary, Pen darkPrimary, Pen lightPrimary, Pen accent, Pen textShade)
            : base(primary, darkPrimary, lightPrimary, accent, textShade)
        {
        }

    }
}
