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
    /// 复选框离开方案。
    /// </summary>
    public class CheckboxOffScheme
    {
        /// <summary>
        /// 构造一个 <see cref="CheckboxOffScheme"/> 实例。
        /// </summary>
        /// <param name="enableColor">给定的控件启用颜色。</param>
        /// <param name="disabledColor">给定的控件禁用颜色。</param>
        public CheckboxOffScheme(Color enableColor, Color disabledColor)
        {
            Enable = new SchemeInfo(enableColor);
            Disabled = new SchemeInfo(disabledColor);
        }
        /// <summary>
        /// 构造一个 <see cref="CheckboxOffScheme"/> 实例。
        /// </summary>
        /// <param name="enable">给定的控件启用方案。</param>
        /// <param name="disabled">给定的控件禁用方案。</param>
        public CheckboxOffScheme(SchemeInfo enable, SchemeInfo disabled)
        {
            Enable = enable;
            Disabled = disabled;
        }


        /// <summary>
        /// 获取控件启用方案。
        /// </summary>
        public SchemeInfo Enable { get; }

        /// <summary>
        /// 获取控件禁用方案。
        /// </summary>
        public SchemeInfo Disabled { get; }


        #region Defaults

        /// <summary>
        /// 解析方案。
        /// </summary>
        /// <param name="schemeType">给定的方案类型。</param>
        /// <returns>返回 <see cref="CheckboxOffScheme"/>。</returns>
        public static CheckboxOffScheme Parse(SchemeType schemeType)
        {
            return (schemeType == SchemeType.BlackOrDark ? Dark : Light);
        }


        /// <summary>
        /// 获取暗系方案。
        /// </summary>
        public static readonly CheckboxOffScheme Dark = new CheckboxOffScheme(Color.FromArgb(179, 255, 255, 255),
            Color.FromArgb(77, 255, 255, 255));

        /// <summary>
        /// 获取亮系方案。
        /// </summary>
        public static readonly CheckboxOffScheme Light = new CheckboxOffScheme(Color.FromArgb(138, 0, 0, 0),
            Color.FromArgb(66, 0, 0, 0));

        #endregion

    }
}
