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
using SkiaSharp;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Drawing.Services
{
    using Core.Combiners;
    using Core.Services;
    using Core.Utilities;
    using Drawing.Builders;
    using Drawing.Resources;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class WatermarkService : AbstractExtensionBuilderService<DrawingBuilderOptions>, IWatermarkService
    {
        public WatermarkService(DrawingBuilderDependency dependency,
            ILoggerFactory loggerFactory)
            : base(dependency.Options, loggerFactory)
        {
            ImageFilePathCombiner = new FilePathCombiner(Options.Watermark.ImagePath);
            ImageFilePathCombiner.ChangeBasePathIfEmpty(dependency.BaseDirectory);

            FontFilePathCombiner = new FilePathCombiner(Options.Watermark.Font.FilePath);
            FontFilePathCombiner.ChangeBasePathIfEmpty(dependency.BaseDirectory);
        }

        
        public FilePathCombiner ImageFilePathCombiner { get; }

        public FilePathCombiner FontFilePathCombiner { get; }

        public SKEncodedImageFormat CurrentImageFormat
            => Options.ImageFormat.MatchEnum<ImageFormat, SKEncodedImageFormat>();


        public Task<bool> DrawFileAsync(string imagePath, string savePath, WatermarkMode mode = WatermarkMode.Text, CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
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

                return result;
            });
        }


        public Task<bool> DrawStreamAsync(string imagePath, Stream target, WatermarkMode mode = WatermarkMode.Text, CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                var result = false;

                DrawCore(imagePath, mode, data =>
                {
                    data.SaveTo(target);

                    Logger.LogDebug($"Watermark image file save as stream");
                    result = true;
                });

                return result;
            });
        }


        public Task<byte[]> DrawBytesAsync(string imagePath, WatermarkMode mode = WatermarkMode.Text, CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                var buffer = default(byte[]);

                DrawCore(imagePath, mode, data =>
                {
                    buffer = data.ToArray();
                    Logger.LogDebug($"Watermark image file save as byte[]: length={buffer.Length}");
                });

                return buffer;
            });
        }


        public void DrawCore(string imagePath, WatermarkMode mode, Action<SKData> postAction)
        {
            Logger.LogDebug($"Watermark image file: {imagePath}");
            Logger.LogDebug($"Watermark mode: {mode.AsEnumName()}");

            using (var bmp = SKBitmap.Decode(imagePath))
            {
                using (var canvas = new SKCanvas(bmp))
                {
                    var imageSize = new Size(bmp.Width, bmp.Height);

                    DrawCore(canvas, imageSize, mode);
                }

                using (var img = SKImage.FromBitmap(bmp))
                using (var data = img.Encode(CurrentImageFormat, Options.Quality))
                {
                    if (data.IsNull())
                        throw new InvalidOperationException(InternalResource.InvalidOperationExceptionUnsupportedImageFormat);

                    postAction.Invoke(data);
                }
            }
        }


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

            // 如果使用随机坐标水印
            if (Options.Watermark.IsRandom)
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
            
            switch (mode)
            {
                case WatermarkMode.Text:
                    {
                        using (var foreFont = CreatePaint(Options.Watermark.Colors.Fore))
                        using (var alternFont = CreatePaint(Options.Watermark.Colors.Alternate))
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
                                    i % 2 > 0 ? alternFont : foreFont);

                                // 递增字符宽度
                                startX += (int)rect.Width;
                            }
                        }
                    }
                    break;

                case WatermarkMode.Image:
                    {
                        using (var watermark = SKBitmap.Decode(ImageFilePathCombiner.ToString()))
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
        

        private SKPaint CreatePaint(SKColor color)
        {
            var paint = new SKPaint();
            paint.IsAntialias = true;
            paint.Color = color;
            // paint.StrokeCap = SKStrokeCap.Round;
            paint.Typeface = SKTypeface.FromFile(FontFilePathCombiner.ToString());
            paint.TextSize = Options.Watermark.Font.Size;

            return paint;
        }

    }
}
