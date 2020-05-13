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
    using Core.Builders;
    using Core.Combiners;
    using Core.Services;
    using Drawing.Builders;
    using Drawing.Resources;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class WatermarkService : AbstractExtensionBuilderService<DrawingBuilderOptions>, IWatermarkService
    {
        public WatermarkService(DrawingBuilderDependency dependency, ILoggerFactory loggerFactory)
            : base(dependency?.Options, loggerFactory)
        {
            Dependency = dependency;

            ImageFilePathCombiner = new FilePathCombiner(Options.Watermark.ImagePath);
            ImageFilePathCombiner.ChangeBasePathIfEmpty(dependency.ResourceDirectory);

            FontFilePathCombiner = new FilePathCombiner(Options.Watermark.Font.FilePath);
            FontFilePathCombiner.ChangeBasePathIfEmpty(dependency.ResourceDirectory);
        }


        public IExtensionBuilderDependency Dependency { get; }

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
            var coordinate = ImageHelper.CalculateCoordinate(imageSize,
                Options.Watermark.Location, Options.Watermark.IsRandom);
            
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
                                canvas.DrawText(character, coordinate.X + (int)rect.Width, coordinate.Y,
                                    i % 2 > 0 ? alternFont : foreFont);

                                // 递增字符宽度
                                coordinate.X += (int)rect.Width;
                            }
                        }
                    }
                    break;

                case WatermarkMode.Image:
                    {
                        using (var watermark = SKBitmap.Decode(ImageFilePathCombiner.ToString()))
                        {
                            // 绘制图像水印
                            canvas.DrawBitmap(watermark, coordinate.X, coordinate.Y);
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
