#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SkiaSharp;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Drawing
{
    using Core;

    /// <summary>
    /// 内部水印服务。
    /// </summary>
    internal class InternalWatermarkService : AbstractService<InternalWatermarkService, DrawingBuilderOptions>, IWatermarkService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalWatermarkService"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{DrawingBuilderOptions}"/></param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalWatermarkService}"/>。</param>
        public InternalWatermarkService(IOptions<DrawingBuilderOptions> options,
            ILogger<InternalWatermarkService> logger)
            : base(options, logger)
        {
        }

        
        /// <summary>
        /// 水印图像文件定位器。
        /// </summary>
        public IFileLocator ImageFileLocator => Options.Watermark.ImageFileLocator;

        /// <summary>
        /// 水印字体文件定位器。
        /// </summary>
        public IFileLocator FontFileLocator => Options.Watermark.Font.FileLocator;


        /// <summary>
        /// 绘制水印文件。
        /// </summary>
        /// <param name="imagePath">给定的图像路径。</param>
        /// <param name="savePath">给定的保存路径。</param>
        /// <param name="mode">给定的水印模绘制式（可选；默认使用文本模式）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否成功的异步操作。</returns>
        public Task<bool> DrawFile(string imagePath, string savePath, WatermarkMode mode = WatermarkMode.Text, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = false;
            
            DrawCore(imagePath, mode, data =>
            {
                using (var fs = new FileStream(savePath, FileMode.OpenOrCreate))
                {
                    data.SaveTo(fs);
                }

                Logger.LogDebug($"Watermark image file save as: {savePath}");
                result = true;
            });

            return Task.FromResult(result);
        }


        /// <summary>
        /// 绘制水印流。
        /// </summary>
        /// <param name="imagePath">给定的验证码。</param>
        /// <param name="target">给定的目标流。</param>
        /// <param name="mode">给定的水印模绘制式（可选；默认使用文本模式）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否成功的异步操作。</returns>
        public Task<bool> DrawStream(string imagePath, Stream target, WatermarkMode mode = WatermarkMode.Text, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = false;
            
            DrawCore(imagePath, mode, data =>
            {
                data.SaveTo(target);

                Logger.LogDebug($"Watermark image file save as stream");
                result = true;
            });

            return Task.FromResult(result);
        }


        /// <summary>
        /// 绘制水印字节数组。
        /// </summary>
        /// <param name="imagePath">给定的图像路径。</param>
        /// <param name="mode">给定的水印模绘制式（可选；默认使用文本模式）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含图像字节数组的异步操作。</returns>
        public Task<byte[]> DrawBytes(string imagePath, WatermarkMode mode = WatermarkMode.Text, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var buffer = default(byte[]);
            
            DrawCore(imagePath, mode, data =>
            {
                buffer = data.ToArray();
                Logger.LogDebug($"Watermark image file save as byte[]: length={buffer.Length}");
            });

            return Task.FromResult(buffer);
        }


        /// <summary>
        /// 绘制水印。
        /// </summary>
        /// <param name="imagePath">给定的图像路径。</param>
        /// <param name="mode">给定的水印模式。</param>
        /// <param name="postAction">后置动作处理。</param>
        public void DrawCore(string imagePath, WatermarkMode mode, Action<SKData> postAction)
        {
            Logger.LogDebug($"Watermark image file: {imagePath}");
            Logger.LogDebug($"Watermark mode: {mode.AsEnumName()}");
            
            var skFormat = Options.ImageFormat.AsOutputEnumByName<ImageFormat, SKEncodedImageFormat>();
            
            using (var bmp = SKBitmap.Decode(imagePath))
            {
                using (var canvas = new SKCanvas(bmp))
                {
                    var imageSize = new Size(bmp.Width, bmp.Height);

                    DrawCore(canvas, imageSize, mode);
                }

                using (var img = SKImage.FromBitmap(bmp))
                {
                    using (var data = img.Encode(skFormat, Options.Quality))
                    {
                        postAction.Invoke(data);
                    }
                }
            }
        }


        /// <summary>
        /// 绘制水印。
        /// </summary>
        /// <param name="canvas">给定的图像处理上下文。</param>
        /// <param name="imageSize">给定的图像尺寸。</param>
        /// <param name="mode">给定的水印模式。</param>
        internal void DrawCore(SKCanvas canvas, Size imageSize, WatermarkMode mode)
        {
            var startX = Options.Watermark.Location.X;
            var startY = Options.Watermark.Location.Y;
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

            // 如果使用随机位置水印
            if (Options.Watermark.IsRandom)
            {
                var random = new Random();

                if (isReverseX)
                    startX = random.Next(startX, imageSize.Width - Math.Abs(startX));
                else
                    startX = random.Next(startX, imageSize.Width / 2);

                if (isReverseY)
                    startY = random.Next(startY, imageSize.Height - Math.Abs(startY));
                else
                    startY = random.Next(startY, imageSize.Height / 2);
            }
            
            switch (mode)
            {
                case WatermarkMode.Text:
                    {
                        using (var foreFont = CreateFontPaint(Options.Watermark.Colors.ForeHex))
                        using (var alterFont = string.IsNullOrEmpty(Options.Watermark.Colors.AlternateHex)
                            ? foreFont : CreateFontPaint(Options.Watermark.Colors.AlternateHex))
                        {
                            var text = Options.Watermark.Text;

                            for (int i = 0; i < text.Length; i++)
                            {
                                // 当前字符
                                var character = text.Substring(i, 1);

                                // 测算水印文本内容矩形尺寸
                                var rect = new SKRect();
                                foreFont.MeasureText(character, ref rect);

                                // 绘制文本水印
                                canvas.DrawText(character, startX + (int)rect.Width, startY,
                                    (i % 2 > 0 ? alterFont : foreFont));

                                // 递增字符宽度
                                startX += (int)rect.Width;
                            }
                        }
                    }
                    break;

                case WatermarkMode.Image:
                    {
                        using (var watermark = SKBitmap.Decode(ImageFileLocator.ToString()))
                        {
                            // 绘制图像水印
                            canvas.DrawBitmap(watermark, startX, startY);
                        }
                    }
                    break;

                default:
                    break;
            }
        }
        

        /// <summary>
        /// 创建字体绘画。
        /// </summary>
        /// <param name="colorHexString">给定颜色的 16 进制字符串。</param>
        /// <returns>返回 <see cref="SKPaint"/>。</returns>
        private SKPaint CreateFontPaint(string colorHexString)
        {
            var paint = new SKPaint();
            paint.IsAntialias = true;
            paint.Color = SKColor.Parse(colorHexString);
            // paint.StrokeCap = SKStrokeCap.Round;
            paint.Typeface = SKTypeface.FromFile(FontFileLocator.ToString());
            paint.TextSize = Options.Watermark.Font.Size;

            return paint;
        }

    }
}
