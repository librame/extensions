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

namespace Librame.Extensions.Drawing
{
    /// <summary>
    /// 图像助手。
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// 缩放大小。
        /// </summary>
        /// <param name="rawSize">给定的原始大小。</param>
        /// <param name="maxSize">给定的最大大小。</param>
        /// <returns>返回经过缩放的 <see cref="Size"/>。</returns>
        public static Size ScaleSize(Size rawSize, Size maxSize)
        {
            // 缩略图宽、高计算
            double zoomWidth = rawSize.Width;
            double zoomHeight = rawSize.Height;

            // 宽大于高或宽等于高（横图或正方）
            if (rawSize.Width > rawSize.Height || rawSize.Width == rawSize.Height)
            {
                // 如果宽大于模版
                if (rawSize.Width > maxSize.Width)
                {
                    // 宽按模版，高按比例缩放
                    zoomWidth = maxSize.Width;
                    zoomHeight = zoomHeight * ((double)maxSize.Width / rawSize.Width);
                }
            }
            // 高大于宽（竖图）
            else
            {
                // 如果高大于模版
                if (rawSize.Height > maxSize.Height)
                {
                    // 高按模版，宽按比例缩放
                    zoomHeight = maxSize.Height;
                    zoomWidth = zoomWidth * ((double)maxSize.Height / rawSize.Height);
                }
            }

            return new Size((int)zoomWidth, (int)zoomHeight);
        }

    }
}
