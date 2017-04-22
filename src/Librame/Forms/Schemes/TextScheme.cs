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
    /// 文本方案。
    /// </summary>
    public class TextScheme
    {
        /// <summary>
        /// 构造一个 <see cref="TextScheme"/> 实例。
        /// </summary>
        /// <param name="primaryColor">给定的主颜色。</param>
        /// <param name="secondaryColor">给定的次颜色。</param>
        /// <param name="disabledOrHintColor">给定的禁用或暗示颜色。</param>
        /// <param name="dividersColor">给定的分隔器颜色。</param>
        public TextScheme(Color primaryColor, Color secondaryColor, Color disabledOrHintColor, Color dividersColor)
        {
            Primary = new SchemeInfo(primaryColor);
            Secondary = new SchemeInfo(secondaryColor);
            DisabledOrHint = new SchemeInfo(disabledOrHintColor);
            Dividers = new SchemeInfo(dividersColor);
        }


        /// <summary>
        /// 获取主方案信息。
        /// </summary>
        public SchemeInfo Primary { get; }

        /// <summary>
        /// 获取次方案信息。
        /// </summary>
        public SchemeInfo Secondary { get; }

        /// <summary>
        /// 获取禁用或暗示方案信息。
        /// </summary>
        public SchemeInfo DisabledOrHint { get; }

        /// <summary>
        /// 获取分隔器方案信息。
        /// </summary>
        public SchemeInfo Dividers { get; }


        #region Defaults

        /// <summary>
        /// 解析方案。
        /// </summary>
        /// <param name="schemeType">给定的方案类型。</param>
        /// <returns>返回 <see cref="TextScheme"/>。</returns>
        public static TextScheme Parse(SchemeType schemeType)
        {
            return (schemeType == SchemeType.BlackOrDark ? Black : White);
        }


        /// <summary>
        /// 获取黑色方案。
        /// </summary>
        public static readonly TextScheme Black = new TextScheme(Color.FromArgb(222, 0, 0, 0),
            Color.FromArgb(138, 0, 0, 0),
            Color.FromArgb(66, 0, 0, 0),
            Color.FromArgb(31, 0, 0, 0));

        /// <summary>
        /// 获取白色方案。
        /// </summary>
        public static readonly TextScheme White = new TextScheme(Color.FromArgb(255, 255, 255, 255),
            Color.FromArgb(179, 255, 255, 255),
            Color.FromArgb(77, 255, 255, 255),
            Color.FromArgb(31, 255, 255, 255));

        #endregion

    }
}
