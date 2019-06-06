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
using System.Threading.Tasks;

namespace Librame.Extensions.Drawing
{
    using Core;

    /// <summary>
    /// 内部验证码服务。
    /// </summary>
    internal class InternalCaptchaService : AbstractDrawingService<InternalCaptchaService>, ICaptchaService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalCaptchaService"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{DrawingBuilderOptions}"/></param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalCaptchaService}"/>。</param>
        public InternalCaptchaService(IOptions<DrawingBuilderOptions> options, ILogger<InternalCaptchaService> logger)
            : base(options, logger)
        {
        }


        /// <summary>
        /// 字体文件定位器。
        /// </summary>
        public IFileLocator FontFileLocator => Options.Captcha.Font.FileLocator;


        /// <summary>
        /// 绘制验证码文件。
        /// </summary>
        /// <param name="captcha">给定的验证码。</param>
        /// <param name="savePath">给定的保存路径。</param>
        /// <returns>返回一个包含是否成功的异步操作。</returns>
        public Task<bool> DrawFile(string captcha, string savePath)
        {
            var result = false;

            DrawCore(captcha, data =>
            {
                using (var fs = new FileStream(savePath, FileMode.OpenOrCreate))
                {
                    data.SaveTo(fs);
                }

                Logger.LogDebug($"Captcha image file save as: {savePath}");
                result = true;
            });

            return Task.FromResult(result);
        }


        /// <summary>
        /// 绘制验证码流。
        /// </summary>
        /// <param name="captcha">给定的验证码。</param>
        /// <param name="target">给定的目标流。</param>
        /// <returns>返回一个包含是否成功的异步操作。</returns>
        public Task<bool> DrawStream(string captcha, Stream target)
        {
            var result = false;

            DrawCore(captcha, data =>
            {
                data.SaveTo(target);
                
                Logger.LogDebug($"Captcha image save as stream");
                result = true;
            });

            return Task.FromResult(result);
        }


        /// <summary>
        /// 绘制验证码字节数组。
        /// </summary>
        /// <param name="captcha">给定的验证码。</param>
        /// <returns>返回一个包含图像字节数组的异步操作。</returns>
        public Task<byte[]> DrawBytes(string captcha)
        {
            var buffer = default(byte[]);

            DrawCore(captcha, data =>
            {
                buffer = data.ToArray();
                Logger.LogDebug($"Captcha image save as byte[]: length={buffer.Length}");
            });

            return Task.FromResult(buffer);
        }


        /// <summary>
        /// 绘制验证码。
        /// </summary>
        /// <param name="captcha">给定的验证码。</param>
        /// <param name="postAction">后置动作处理。</param>
        public void DrawCore(string captcha, Action<SKData> postAction)
        {
            Logger.LogDebug($"Captcha text: {captcha}");

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


        /// <summary>
        /// 计算验证码尺寸和字符定位集合。
        /// </summary>
        /// <param name="captcha">给定的验证码。</param>
        /// <returns>返回尺寸和字符定位集合。</returns>
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
            paint.Typeface = SKTypeface.FromFile(FontFileLocator.GetSource());
            paint.TextSize = Options.Watermark.Font.Size;

            return paint;
        }


        /// <summary>
        /// 解析噪点字体绘画。
        /// </summary>
        /// <returns>返回字体对象。</returns>
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


        /// <summary>
        /// 创建噪点集合。
        /// </summary>
        /// <param name="imageSize">给定的图片尺寸。</param>
        /// <returns>返回噪点集合。</returns>
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
