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
    /// 上下文菜单带方案。
    /// </summary>
    public class ContextMenuStripScheme : SchemeInfo
    {
        /// <summary>
        /// 构造一个 <see cref="ContextMenuStripScheme"/> 实例。
        /// </summary>
        /// <param name="color">给定的背景颜色。</param>
        public ContextMenuStripScheme(Color color)
            : base(color)
        {
        }


        #region Defaults

        /// <summary>
        /// 解析方案。
        /// </summary>
        /// <param name="schemeType">给定的方案类型。</param>
        /// <returns>返回 <see cref="ContextMenuStripScheme"/>。</returns>
        public static ContextMenuStripScheme Parse(SchemeType schemeType)
        {
            return (schemeType == SchemeType.BlackOrDark ? DarkHover : LightHover);
        }


        /// <summary>
        /// 获取暗系悬停方案。
        /// </summary>
        public static readonly ContextMenuStripScheme DarkHover = new ContextMenuStripScheme(Color.FromArgb(38, 204, 204, 204));

        /// <summary>
        /// 获取亮系悬停方案。
        /// </summary>
        public static readonly ContextMenuStripScheme LightHover = new ContextMenuStripScheme(Color.FromArgb(255, 238, 238, 238));

        #endregion

    }
}
