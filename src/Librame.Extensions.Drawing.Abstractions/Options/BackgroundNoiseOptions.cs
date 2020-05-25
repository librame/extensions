#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
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
    public class BackgroundNoiseOptions
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
