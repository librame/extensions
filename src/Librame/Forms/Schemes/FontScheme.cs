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
    /// 字体方案。
    /// </summary>
    public class FontScheme
    {
        /// <summary>
        /// 构造一个 <see cref="FontScheme"/> 实例。
        /// </summary>
        /// <param name="familyName">给定的字体名称。</param>
        public FontScheme(string familyName)
        {
            Medium10 = new Font(familyName, 10f);
            Medium11 = new Font(familyName, 11f);
            Medium12 = new Font(familyName, 12f);
            Regular11 = new Font(familyName, 11f, FontStyle.Regular);
        }
        /// <summary>
        /// 构造一个 <see cref="FontScheme"/> 实例。
        /// </summary>
        /// <param name="medium10">给定的 10 号中等字体。</param>
        /// <param name="medium11">给定的 11 号中等字体。</param>
        /// <param name="medium12">给定的 12 号中等字体。</param>
        /// <param name="regular11">给定的 11 号规则字体。</param>
        public FontScheme(Font medium10, Font medium11, Font medium12, Font regular11)
        {
            Medium10 = medium10;
            Medium11 = medium11;
            Medium12 = medium12;
            Regular11 = regular11;
        }


        /// <summary>
        /// 获取 10 号中等字体。
        /// </summary>
        public Font Medium10 { get; }

        /// <summary>
        /// 获取 11 号中等字体。
        /// </summary>
        public Font Medium11 { get; }

        /// <summary>
        /// 获取 12 号中等字体。
        /// </summary>
        public Font Medium12 { get; }

        /// <summary>
        /// 获取 11 号规则字体。
        /// </summary>
        public Font Regular11 { get; }


        #region Defaults

        /// <summary>
        /// 获取 Arial 字体方案。
        /// </summary>
        public static readonly FontScheme Arial = new FontScheme("Arial");

        /// <summary>
        /// 获取 SegoeUI 字体方案。
        /// </summary>
        public static readonly FontScheme SegoeUI = new FontScheme("Segoe UI");

        /// <summary>
        /// 获取 Verdana 字体方案。
        /// </summary>
        public static readonly FontScheme Verdana = new FontScheme("Verdana");

        #endregion

    }
}
