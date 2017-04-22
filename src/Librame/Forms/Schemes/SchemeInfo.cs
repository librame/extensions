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
    /// 方案信息。
    /// </summary>
    public class SchemeInfo
    {
        /// <summary>
        /// 构造一个 <see cref="SchemeInfo"/> 实例。
        /// </summary>
        /// <param name="color">给定的颜色。</param>
        public SchemeInfo(Color color)
        {
            Color = color;
            Brush = new SolidBrush(color);
        }


        /// <summary>
        /// 获取颜色。
        /// </summary>
        public Color Color { get; }

        /// <summary>
        /// 获取笔刷。
        /// </summary>
        public Brush Brush { get; protected set; }

    }
}
