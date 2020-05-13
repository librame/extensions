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

namespace Librame.Extensions.Drawing.Options
{
    /// <summary>
    /// 背景噪点选项。
    /// </summary>
    public class NoiseOptions
    {
        /// <summary>
        /// 宽度。
        /// </summary>
        public int Width { get; set; }
            = 2;

        /// <summary>
        /// 间距。
        /// </summary>
        public PointF Space { get; set; }
            = new PointF
            {
                X = 5,
                Y = 5
            };
    }
}
