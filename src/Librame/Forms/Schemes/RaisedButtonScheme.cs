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
    /// 浮雕按钮方案。
    /// </summary>
    public class RaisedButtonScheme
    {
        /// <summary>
        /// 构造一个 <see cref="RaisedButtonScheme"/> 实例。
        /// </summary>
        /// <param name="textColor">给定的文本颜色。</param>
        /// <param name="backgroundColor">给定的背景颜色。</param>
        public RaisedButtonScheme(Color textColor, Color backgroundColor)
        {
            Text = new SchemeInfo(textColor);
            Background = new SchemeInfo(backgroundColor);
        }


        /// <summary>
        /// 获取文本方案。
        /// </summary>
        public SchemeInfo Text { get; }

        /// <summary>
        /// 获取背景方案。
        /// </summary>
        public SchemeInfo Background { get; }


        #region Defaults

        /// <summary>
        /// 解析方案。
        /// </summary>
        /// <param name="schemeType">给定的方案类型。</param>
        /// <returns>返回 <see cref="RaisedButtonScheme"/>。</returns>
        public static RaisedButtonScheme Parse(SchemeType schemeType)
        {
            return (schemeType == SchemeType.BlackOrDark ? Dark : Light);
        }


        /// <summary>
        /// 获取暗系方案。
        /// </summary>
        public static readonly RaisedButtonScheme Dark = new RaisedButtonScheme(TextScheme.Black.Primary.Color,
            Color.FromArgb(255, 255, 255, 255));

        /// <summary>
        /// 获取亮系方案。
        /// </summary>
        public static readonly RaisedButtonScheme Light = new RaisedButtonScheme(TextScheme.White.Primary.Color,
            Color.FromArgb(255, 255, 255, 255));

        #endregion

    }
}
