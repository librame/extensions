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
    /// 背景方案。
    /// </summary>
    public class BackgroundScheme : SchemeInfo
    {
        /// <summary>
        /// 构造一个 <see cref="BackgroundScheme"/> 实例。
        /// </summary>
        /// <param name="color">给定的背景颜色。</param>
        public BackgroundScheme(Color color)
            : base(color)
        {
        }


        #region Defaults

        /// <summary>
        /// 解析方案。
        /// </summary>
        /// <param name="schemeType">给定的方案类型。</param>
        /// <returns>返回 <see cref="BackgroundScheme"/>。</returns>
        public static BackgroundScheme Parse(SchemeType schemeType)
        {
            return (schemeType == SchemeType.BlackOrDark ? Dark : Light);
        }


        /// <summary>
        /// 获取暗系背景方案。
        /// </summary>
        public static readonly BackgroundScheme Dark = new BackgroundScheme(Color.FromArgb(255, 51, 51, 51));

        /// <summary>
        /// 获取亮系背景方案。
        /// </summary>
        public static readonly BackgroundScheme Light = new BackgroundScheme(Color.FromArgb(255, 255, 255, 255));

        #endregion

    }
}
