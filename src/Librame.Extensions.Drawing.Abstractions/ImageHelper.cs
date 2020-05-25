#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Drawing;

namespace Librame.Extensions.Drawing
{
    using Core.Utilities;

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

        /// <summary>
        /// 计算图像坐标。
        /// </summary>
        /// <param name="imageSize">给定的图像。</param>
        /// <param name="initialCoordinate">给定的初始化坐标。</param>
        /// <param name="isRandomTransformation">是否随机变换坐标（可选；默认不随机变换）。</param>
        /// <returns>返回 <see cref="Point"/>。</returns>
        public static Point CalculateCoordinate(Size imageSize, Point initialCoordinate,
            bool isRandomTransformation = false)
        {
            var startX = initialCoordinate.X;
            var startY = initialCoordinate.Y;
            
            var isReverseX = false;
            var isReverseY = false;

            // 如果为负值，则表示反向
            if (startX < 0)
            {
                startX = imageSize.Width / 2 - Math.Abs(startX);
                isReverseX = true;
            }

            if (startY < 0)
            {
                startY = imageSize.Height / 2 - Math.Abs(startY);
                isReverseY = true;
            }

            // 如果使用随机变换坐标
            if (isRandomTransformation)
            {
                RandomUtility.Run(r =>
                {
                    if (isReverseX)
                        startX = r.Next(startX, imageSize.Width - Math.Abs(startX));
                    else
                        startX = r.Next(startX, imageSize.Width / 2);

                    if (isReverseY)
                        startY = r.Next(startY, imageSize.Height - Math.Abs(startY));
                    else
                        startY = r.Next(startY, imageSize.Height / 2);
                });
            }

            return new Point(startX, startY);
        }

    }
}
