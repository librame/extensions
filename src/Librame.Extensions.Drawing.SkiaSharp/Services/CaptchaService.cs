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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Drawing
{
    using Core;

    class CaptchaService : AbstractExtensionBuilderService<DrawingBuilderOptions>, ICaptchaService
    {
        public CaptchaService(IOptions<DrawingBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }


        public FilePathCombiner FontFilePathCombiner => Options.Captcha.Font.FilePath;


        public Task<bool> DrawFileAsync(string captcha, string savePath, CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                DrawCore(captcha, data =>
                {
                    using (var fs = new FileStream(savePath, FileMode.OpenOrCreate))
                    {
                        data.SaveTo(fs);
                    }

                    Logger.LogInformation($"Captcha image file save as: {savePath}");
                });

                return File.Exists(savePath);
            });
        }


        public Task<bool> DrawStreamAsync(string captcha, Stream target, CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                DrawCore(captcha, data =>
                {
                    data.SaveTo(target);

                    Logger.LogInformation($"Captcha image save as stream");
                });

                return true;
            });
        }


        public Task<byte[]> DrawBytesAsync(string captcha, CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                var buffer = default(byte[]);

                DrawCore(captcha, data =>
                {
                    buffer = data.ToArray();
                    Logger.LogDebug($"Captcha image save as byte[]: length={buffer.Length}");
                });

                return buffer;
            });
        }


        public void DrawCore(string captcha, Action<SKData> postAction)
        {
            Logger.LogInformation($"Captcha text: {captcha}");

            var colorOptions = Options.Captcha.Colors;
            var bgColor = SKColor.Parse(colorOptions.BackgroundHex);

            var sizeAndPoints = ComputeSizeAndPoints(captcha);
            var imageSize = sizeAndPoints.Size;
            var imageInfo = new SKImageInfo(imageSize.Width, imageSize.Height,
                SKColorType.Bgra8888, SKAlphaType.Premul);
            
            var skFormat = Options.ImageFormat.AsOutputEnumByName<ImageFormat, SKEncodedImageFormat>();

            using (var bmp = new SKBitmap(imageInfo))
            {
                using (var canvas = new SKCanvas(bmp))
                {
                    // Clear
                    canvas.DrawColor(bgColor);

                    // 绘制噪点
                    using (var noiseFont = CreateNoiseFontPaint())
                    {
                        var points = CreateNoisePoints(imageSize);

                        canvas.DrawPoints(SKPointMode.Points, points, noiseFont);
                    }

                    // 绘制验证码
                    using (var foreFont = CreateFontPaint(colorOptions.ForeHex))
                    {
                        using (var alterFont = string.IsNullOrEmpty(colorOptions.AlternateHex)
                            ? foreFont : CreateFontPaint(colorOptions.AlternateHex))
                        {
                            foreach (var p in sizeAndPoints.Points)
                            {
                                var i = p.Key;
                                var character = p.Value.Key;
                                var point = p.Value.Value;

                                canvas.DrawText(character, point.X, point.Y,
                                    (i % 2 > 0 ? alterFont : foreFont));
                            }
                        }
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
        }


        private (Size Size, IDictionary<int, KeyValuePair<string, SKPoint>> Points) ComputeSizeAndPoints(string captcha)
        {
            var points = new Dictionary<int, KeyValuePair<string, SKPoint>>();
            var size = new Size();

            using (var foreFont = CreateFontPaint(Options.Captcha.Colors.ForeHex))
            {
                var paddingHeight = (int)foreFont.TextSize;
                var paddingWidth = paddingHeight / 2;
                
                var startX = paddingWidth;
                var startY = paddingHeight;

                for (int i = 0; i < captcha.Length; i++)
                {
                    // 当前字符
                    var character = captcha.Substring(i, 1);

                    // 测算字符矩形
                    var rect = new SKRect();
                    foreFont.MeasureText(character, ref rect);

                    // 当前字符宽高
                    var charWidth = (int)rect.Width;
                    var charHeight = (int)rect.Height;
                    
                    var point = new SKPoint();

                    // 随机变换其余字符坐标
                    var random = new Random();
                    point.X = random.Next(startX, charWidth + startX);
                    point.Y = random.Next(startY, charHeight + startY);

                    // 附加为字符宽度加当前字符横坐标
                    startX = (int)point.X + charWidth + paddingWidth;

                    points.Add(i, new KeyValuePair<string, SKPoint>(character, point));
                }

                size.Width += startX + paddingWidth;
                size.Height += startY + paddingHeight;
            }

            return (size, points);
        }


        private SKPaint CreateFontPaint(string colorHexString)
        {
            var paint = new SKPaint();
            paint.IsAntialias = true;
            paint.Color = SKColor.Parse(colorHexString);
            // paint.StrokeCap = SKStrokeCap.Round;
            paint.Typeface = SKTypeface.FromFile(FontFilePathCombiner.ToString());
            paint.TextSize = Options.Watermark.Font.Size;

            return paint;
        }


        private SKPaint CreateNoiseFontPaint()
        {
            var colors = Options.Captcha.Colors;
            var noises = Options.Captcha.Noise;

            var paint = new SKPaint();
            paint.IsAntialias = true;
            paint.Color = SKColor.Parse(colors.DisturbingHex);
            paint.StrokeCap = SKStrokeCap.Square;
            paint.StrokeWidth = noises.Width;

            return paint;
        }


        private SKPoint[] CreateNoisePoints(Size imageSize)
        {
            var noises = Options.Captcha.Noise;

            var points = new List<SKPoint>();

            var offset = noises.Width;
            var xCount = imageSize.Width / noises.Space.X + offset;
            var yCount = imageSize.Height / noises.Space.Y + offset;

            for (int i = 0; i < xCount; i++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    var point = new SKPoint(i * noises.Space.X, j * noises.Space.Y);
                    points.Add(point);
                }
            }

            return points.ToArray();
        }

    }
}
