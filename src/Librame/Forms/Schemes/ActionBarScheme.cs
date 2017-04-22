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
    /// 动作栏方案。
    /// </summary>
    public class ActionBarScheme
    {
        /// <summary>
        /// 构造一个 <see cref="ActionBarScheme"/> 实例。
        /// </summary>
        /// <param name="primaryColor">给定的主要颜色。</param>
        /// <param name="secondaryColor">给定的次要颜色。</param>
        public ActionBarScheme(Color primaryColor, Color secondaryColor)
        {
            Primary = new SchemeInfo(primaryColor);
            Secondary = new SchemeInfo(secondaryColor);
        }
        /// <summary>
        /// 构造一个 <see cref="ActionBarScheme"/> 实例。
        /// </summary>
        /// <param name="primary">给定的主要方案。</param>
        /// <param name="secondary">给定的次要方案。</param>
        public ActionBarScheme(SchemeInfo primary, SchemeInfo secondary)
        {
            Primary = primary;
            Secondary = secondary;
        }


        /// <summary>
        /// 获取主要方案。
        /// </summary>
        public SchemeInfo Primary { get; }

        /// <summary>
        /// 获取次要方案。
        /// </summary>
        public SchemeInfo Secondary { get; }


        #region Defaults

        /// <summary>
        /// 获取动作栏默认方案。
        /// </summary>
        public static readonly ActionBarScheme Default = new ActionBarScheme(Color.FromArgb(255, 255, 255, 255),
            Color.FromArgb(153, 255, 255, 255));

        #endregion

    }
}
