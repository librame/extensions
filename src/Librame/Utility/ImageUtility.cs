#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="Image"/> 实用工具。
    /// </summary>
    public class ImageUtility
    {
        /// <summary>
        /// JPEG 多用途互联网邮件扩展类型。
        /// </summary>
        public const string JPEG_MIME_TYPE = "image/jpeg";


        /// <summary>
        /// 获取编解码器信息。
        /// </summary>
        /// <param name="mimeType">给定的 MIME 类型（可选；默认为 JPEG 格式）。</param>
        /// <returns>返回图像编解码器信息。</returns>
        public static ImageCodecInfo GetCodecInfo(string mimeType = JPEG_MIME_TYPE)
        {
            return ImageCodecInfo.GetImageEncoders().First(c => c.MimeType == mimeType);
        }

        
        /// <summary>
        /// 转换为渐进式编码图片。
        /// </summary>
        /// <param name="image">给定的图像。</param>
        /// <param name="saveFileName">给定的另存为图片文件名。</param>
        public static void AsProgressive(Image image, string saveFileName)
        {
            var codec = GetCodecInfo();

            var parameters = new EncoderParameters(3);

            parameters.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
            parameters.Param[1] = new EncoderParameter(Encoder.ScanMethod,
                (int)EncoderValue.ScanMethodInterlaced);
            parameters.Param[2] = new EncoderParameter(Encoder.RenderMethod,
                (int)EncoderValue.RenderProgressive);

            image.Save(saveFileName, codec, parameters);
        }

        
        /// <summary>
        /// 压缩图像。
        /// </summary>
        /// <param name="image">给定的图像。</param>
        /// <param name="saveFileName">给定的另存为图片文件名。</param>
        /// <param name="flag">给定的压缩比例（可选；默认80，范围在1-100之间）。</param>
        /// <param name="mimeType">给定的 MIME 类型（可选；默认为 JPEG 格式）。</param>
        public static void Compress(Image image, string saveFileName, int flag = 80,
            string mimeType = JPEG_MIME_TYPE)
        {
            flag = flag.Range(1, 100);

            var codec = GetCodecInfo(mimeType);

            var parameters = new EncoderParameters();
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, new long[] { flag });

            image.Save(saveFileName, codec, parameters);
        }


        /// <summary>
        /// 绘制图像验证码。
        /// </summary>
        /// <param name="captcha">给定的验证码。</param>
        /// <param name="startP">给定绘制起点的坐标对。</param>
        /// <param name="descriptor">给定的文本绘制描述符。</param>
        /// <returns>返回 <see cref="Bitmap"/>。</returns>
        public static Bitmap Captcha(string captcha, Point startP, TextDrawDescriptor descriptor = null)
        {
            if (ReferenceEquals(descriptor, null))
                descriptor = TextDrawDescriptor.Default;

            // 测算验证码尺寸
            var captchaSize = TextRenderer.MeasureText(captcha, descriptor.Font);

            // 随机生成器
            var random = new Random();

            // 创建验证码图像
            var image = new Bitmap(captchaSize.Width, captchaSize.Height);

            using (var g = Graphics.FromImage(image))
            {
                // 质量设定
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // 清空画布
                g.Clear(descriptor.BgColor);

                // 画图片的干扰线
                for (int i = 0; i < 15; i++)
                {
                    int x1 = random.Next(captchaSize.Width);
                    int x2 = random.Next(captchaSize.Width);
                    int y1 = random.Next(captchaSize.Height);
                    int y2 = random.Next(captchaSize.Height);

                    g.DrawLine(new Pen(descriptor.LineColor), x1, y1, x2, y2);
                }

                // 渐变笔刷
                // var brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                //  Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(captcha, descriptor.Font, new SolidBrush(descriptor.ForeColor), startP.X, startP.Y);

                // 设定前景干扰点
                for (int i = 0; i < 50; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                // 画图片的边框线
                //g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                // 保存
                g.Save();
            }

            return image;
        }


        /// <summary>
        /// 剪切图像。
        /// </summary>
        /// <param name="rawImage">给定的原始图像。</param>
        /// <param name="cutRectangle">给定的剪切矩形。</param>
        /// <returns>返回 <see cref="Bitmap"/>。</returns>
        public static Bitmap Cut(Image rawImage, Rectangle cutRectangle)
        {
            // 创建剪切图像
            var cutImage = new Bitmap(cutRectangle.Width, cutRectangle.Height);

            // 创建以原图像剪切尺寸的重绘图像
            using (var repaintImage = new Bitmap(cutRectangle.Width, cutRectangle.Height))
            {
                using (var g = Graphics.FromImage(repaintImage))
                {
                    // 质量设定
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    // Repaint the rawImage
                    g.DrawImage(rawImage,
                        new Rectangle(0, 0, cutRectangle.Width, cutRectangle.Height),
                        new Rectangle(0, 0, rawImage.Width, rawImage.Height), GraphicsUnit.Pixel);

                    // 保存
                    g.Save();
                }

                // 复制重绘图像到剪切图像
                using (var g = Graphics.FromImage(cutImage))
                {
                    g.DrawImage(repaintImage, 0, 0, cutRectangle, GraphicsUnit.Pixel);

                    // 保存
                    g.Save();
                }
            }

            return cutImage;
        }


        #region Rotate

        /// <summary>
        /// 旋转图像。
        /// </summary>
        /// <param name="rawImage">给定的原始图像。</param>
        /// <param name="angle">给定的旋转角度（仅支持 -180、-90、90、180 等四种角度）。</param>
        /// <returns>返回 <see cref="Bitmap"/>。</returns>
        public static Bitmap Rotate(Image rawImage, int angle)
        {
            // 旋转尺寸
            var rotateSize = RotateResize(rawImage.Size, angle);

            // 创建旋转图像
            var rotateImage = new Bitmap(rotateSize.Width, rotateSize.Height);

            using (var g = Graphics.FromImage(rotateImage))
            {
                // 质量设定
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // 旋转图像
                var offset = new Point((rotateSize.Width - rawImage.Width) / 2,
                    (rotateSize.Height - rawImage.Height) / 2);

                var rect = new Rectangle(offset.X, offset.Y, rawImage.Width, rawImage.Height);
                var center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);

                g.TranslateTransform(center.X, center.Y);
                g.RotateTransform(angle);
                g.TranslateTransform(-center.X, -center.Y);
                g.DrawImage(rawImage, rect);
                g.ResetTransform();

                // 保存
                g.Save();
            }

            return rotateImage;
        }

        /// <summary>
        /// 旋转调整尺寸。
        /// </summary>
        /// <param name="rawSize">给定的原始尺寸。</param>
        /// <param name="angle">给定的旋转角度（仅支持 -180、-90、90、180 等四种角度）。</param>
        /// <returns>返回 <see cref="Size"/>。</returns>
        public static Size RotateResize(Size rawSize, int angle)
        {
            var absAngle = Math.Abs(angle);
            if (absAngle != 90 && absAngle != 180)
                throw new ArgumentException("Angle could only be -180, -90, 90, 180 degrees, but now it is " + angle.ToString() + " degrees!");

            int width = rawSize.Width;
            int height = rawSize.Height;

            // 90/-90
            if (angle == 90)
            {
                width = rawSize.Height;
                height = rawSize.Width;
            }

            return new Size(width, height);
        }

        #endregion


        #region Scale

        /// <summary>
        /// 缩放图像。
        /// </summary>
        /// <param name="rawImage">给定的原图。</param>
        /// <param name="maxSize">给定的最大尺寸。</param>
        /// <param name="bgColor">给定的背景色。</param>
        public static Bitmap ScaleByMaxSize(Image rawImage, Size maxSize, Color bgColor)
        {
            // 缩略尺寸
            var scaleSize = ScaleResize(rawImage.Size, maxSize);

            return Scale(rawImage, scaleSize, bgColor);
        }

        /// <summary>
        /// 缩放图像。
        /// </summary>
        /// <param name="rawImage">给定的原图。</param>
        /// <param name="scaleSize">给定的缩放尺寸。</param>
        /// <param name="bgColor">给定的背景色。</param>
        /// <returns>返回 <see cref="Bitmap"/>。</returns>
        public static Bitmap Scale(Image rawImage, Size scaleSize, Color bgColor)
        {
            // 创建缩略图像
            var scaleImage = new Bitmap(scaleSize.Width, scaleSize.Height);

            using (var g = Graphics.FromImage(scaleImage))
            {
                // 质量设定
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // 清空画布
                g.Clear(bgColor);

                // 绘制缩略图（设置一个单位偏移，否则图片左上角边会出现白色线条）
                int startOffset = 1;

                // 绘制缩略图
                var r = new Rectangle(-startOffset, -startOffset,
                    scaleSize.Width + startOffset, scaleSize.Height + startOffset);

                g.DrawImage(rawImage, r,
                    new Rectangle(0, 0, rawImage.Width, rawImage.Height), GraphicsUnit.Pixel);

                // 保存
                g.Save();
            }

            return scaleImage;
        }
        
        /// <summary>
        /// 等比例缩放调整尺寸。
        /// </summary>
        /// <param name="rawSize">给定的原始尺寸。</param>
        /// <param name="maxSize">给定的目标尺寸。</param>
        /// <returns>返回计算后的缩放尺寸。</returns>
        public static Size ScaleResize(Size rawSize, Size maxSize)
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
                    zoomWidth = (zoomWidth * ((double)maxSize.Height / rawSize.Height));
                }
            }

            return new Size((int)zoomWidth, (int)zoomHeight);
        }

        #endregion


        #region Watermark

        /// <summary>
        /// 计算水印起点坐标。
        /// </summary>
        /// <param name="imgSize">给定的图像尺寸。</param>
        /// <param name="wmSize">给定的水印尺寸。</param>
        /// <param name="startP">给定的起始坐标。</param>
        /// <returns>返回 <see cref="Point"/>。</returns>
        public static Point CalcWatermarkStartP(Size imgSize, Size wmSize, Point startP)
        {
            // 计算绘制的坐标
            int startX = startP.X;
            int startY = startP.Y;

            // 如果坐标为负值，则计算反坐标
            if (startX < 0)
                startX = imgSize.Width - (-startX) - wmSize.Width;

            if (startY < 0)
                startY = imgSize.Height - (-startY) - wmSize.Height;

            return new Point(startX, startY);
        }

        /// <summary>
        /// 附加水印。
        /// </summary>
        /// <param name="rawImage">给定的原图。</param>
        /// <param name="watermark">给定的水印内容（文本或文件名）。</param>
        /// <param name="startP">给定的起始坐标。</param>
        /// <param name="descriptor">如果是文字水印，则需指定文本绘制描述符。</param>
        public static void AppendWatermark(Image rawImage, string watermark, Point startP,
            TextDrawDescriptor descriptor = null)
        {
            if (ReferenceEquals(descriptor, null))
                descriptor = TextDrawDescriptor.Default;

            // 创建绘制图像
            using (var g = Graphics.FromImage(rawImage))
            {
                // 质量设定
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // 如果水印是文件名
                if (watermark.Contains("\\") && File.Exists(watermark))
                    WatermarkFile(g, watermark, rawImage.Size, startP);
                else
                    WatermarkText(g, watermark, rawImage.Size, startP, descriptor);

                // 保存
                g.Save();
            }
        }
        
        private static void WatermarkText(Graphics g, string text, Size imageSize, Point startP,
            TextDrawDescriptor descriptor)
        {
            // 测算文字水印的尺寸
            var textSize = TextRenderer.MeasureText(text, descriptor.Font);

            // 计算水印起点坐标
            var start = CalcWatermarkStartP(imageSize, textSize, startP);

            // 绘制阴影
            int shadowOffset = 2;
            g.DrawString(text, descriptor.Font, new SolidBrush(descriptor.ShadowColor),
                start.X + shadowOffset, start.Y + shadowOffset);

            // 制作文字水印
            g.DrawString(text, descriptor.Font, new SolidBrush(descriptor.ForeColor), start.X, start.Y);
        }
        
        private static void WatermarkFile(Graphics g, string fileName, Size imageSize, Point startP)
        {
            using (var fileImage = Image.FromFile(fileName))
            {
                // 水印绘制条件：原始图像宽高均大于或等于水印图像
                if (imageSize.Width >= fileImage.Width && imageSize.Height >= fileImage.Height)
                {
                    // 设置水印图像背景透明属性
                    ColorMap colorMap = new ColorMap();
                    colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                    colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

                    ColorMap[] remapTable = { colorMap };

                    var attribs = new ImageAttributes();
                    attribs.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                    float[][] colorMatrixElements =
                    {
                        new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                        new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                        new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                        new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f}, // 透明度:0.5
                        new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                    };

                    var wmColorMatrix = new ColorMatrix(colorMatrixElements);
                    attribs.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    // 计算水印起点坐标
                    var start = CalcWatermarkStartP(imageSize, fileImage.Size, startP);

                    // 创建水印图像矩形
                    var r = new Rectangle((int)start.X, (int)start.Y, fileImage.Width, fileImage.Height);

                    // 绘制图像水印
                    g.DrawImage(fileImage, r, 0, 0, fileImage.Width, fileImage.Height, GraphicsUnit.Pixel);
                }
            }
        }

        #endregion

    }


    /// <summary>
    /// <see cref="ImageUtility"/> 静态扩展。
    /// </summary>
    public static class ImageUtilityExtensions
    {
        /// <summary>
        /// 转换为渐进式编码图片。
        /// </summary>
        /// <param name="image">给定的图像。</param>
        /// <param name="saveFileName">给定的另存为图片文件名。</param>
        public static void AsProgressive(this Image image, string saveFileName)
        {
            ImageUtility.AsProgressive(image, saveFileName);
        }


        /// <summary>
        /// 压缩图像。
        /// </summary>
        /// <param name="image">给定的图像。</param>
        /// <param name="saveFileName">给定的另存为图片文件名。</param>
        /// <param name="flag">给定的压缩比例（可选；默认80，范围在1-100之间）。</param>
        /// <param name="mimeType">给定的 MIME 类型（可选；默认为 JPEG 格式）。</param>
        public static void Compress(this Image image, string saveFileName, int flag = 80,
            string mimeType = ImageUtility.JPEG_MIME_TYPE)
        {
            ImageUtility.Compress(image, saveFileName, flag, mimeType);
        }


        /// <summary>
        /// 绘制图像验证码。
        /// </summary>
        /// <param name="captcha">给定的验证码。</param>
        /// <param name="startP">给定绘制起点的坐标对。</param>
        /// <param name="descriptor">给定的文本绘制描述符。</param>
        /// <returns>返回 <see cref="Bitmap"/>。</returns>
        public static Bitmap Captcha(this string captcha, Point startP, TextDrawDescriptor descriptor = null)
        {
            return ImageUtility.Captcha(captcha, startP, descriptor);
        }


        /// <summary>
        /// 剪切图像。
        /// </summary>
        /// <param name="rawImage">给定的原始图像。</param>
        /// <param name="cutRectangle">给定的剪切矩形。</param>
        /// <returns>返回 <see cref="Bitmap"/>。</returns>
        public static Bitmap Cut(this Image rawImage, Rectangle cutRectangle)
        {
            return ImageUtility.Cut(rawImage, cutRectangle);
        }


        #region Rotate

        /// <summary>
        /// 旋转图像。
        /// </summary>
        /// <param name="rawImage">给定的原始图像。</param>
        /// <param name="angle">给定的旋转角度（仅支持 -180、-90、90、180 等四种角度）。</param>
        /// <returns>返回 <see cref="Bitmap"/>。</returns>
        public static Bitmap Rotate(this Image rawImage, int angle)
        {
            return ImageUtility.Rotate(rawImage, angle);
        }

        /// <summary>
        /// 旋转调整尺寸。
        /// </summary>
        /// <param name="rawSize">给定的原始尺寸。</param>
        /// <param name="angle">给定的旋转角度（仅支持 -180、-90、90、180 等四种角度）。</param>
        /// <returns>返回 <see cref="Size"/>。</returns>
        public static Size RotateResize(this Size rawSize, int angle)
        {
            return ImageUtility.RotateResize(rawSize, angle);
        }

        #endregion


        #region Scale

        /// <summary>
        /// 缩放图像。
        /// </summary>
        /// <param name="rawImage">给定的原图。</param>
        /// <param name="maxSize">给定的最大尺寸。</param>
        /// <param name="bgColor">给定的背景色。</param>
        public static Bitmap ScaleByMaxSize(this Image rawImage, Size maxSize, Color bgColor)
        {
            return ImageUtility.ScaleByMaxSize(rawImage, maxSize, bgColor);
        }

        /// <summary>
        /// 缩放图像。
        /// </summary>
        /// <param name="rawImage">给定的原图。</param>
        /// <param name="scaleSize">给定的缩放尺寸。</param>
        /// <param name="bgColor">给定的背景色。</param>
        /// <returns>返回 <see cref="Bitmap"/>。</returns>
        public static Bitmap Scale(this Image rawImage, Size scaleSize, Color bgColor)
        {
            return ImageUtility.Scale(rawImage, scaleSize, bgColor);
        }

        /// <summary>
        /// 等比例缩放调整尺寸。
        /// </summary>
        /// <param name="rawSize">给定的原始尺寸。</param>
        /// <param name="maxSize">给定的目标尺寸。</param>
        /// <returns>返回计算后的缩放尺寸。</returns>
        public static Size ScaleResize(this Size rawSize, Size maxSize)
        {
            return ImageUtility.ScaleResize(rawSize, maxSize);
        }

        #endregion


        #region Watermark

        /// <summary>
        /// 计算水印起点坐标。
        /// </summary>
        /// <param name="imgSize">给定的图像尺寸。</param>
        /// <param name="wmSize">给定的水印尺寸。</param>
        /// <param name="startP">给定的起始坐标。</param>
        /// <returns>返回 <see cref="Point"/>。</returns>
        public static Point CalcWatermarkStartP(this Size imgSize, Size wmSize, Point startP)
        {
            return ImageUtility.CalcWatermarkStartP(imgSize, wmSize, startP);
        }

        /// <summary>
        /// 附加水印。
        /// </summary>
        /// <param name="rawImage">给定的原图。</param>
        /// <param name="watermark">给定的水印内容（文本或文件名）。</param>
        /// <param name="startP">给定的起始坐标。</param>
        /// <param name="descriptor">如果是文字水印，则需指定文本绘制描述符。</param>
        public static void AppendWatermark(this Image rawImage, string watermark, Point startP,
            TextDrawDescriptor descriptor = null)
        {
            ImageUtility.AppendWatermark(rawImage, watermark, startP, descriptor);
        }

        #endregion

    }

}
