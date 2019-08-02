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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Drawing
{
    using Core;

    /// <summary>
    /// 内部缩放服务。
    /// </summary>
    internal class InternalScaleService : ExtensionBuilderServiceBase<DrawingBuilderOptions>, IScaleService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalScaleService"/>。
        /// </summary>
        /// <param name="watermark">给定的 <see cref="IWatermarkService"/>。</param>
        public InternalScaleService(IWatermarkService watermark)
            : base(watermark.CastTo<IWatermarkService, ExtensionBuilderServiceBase<DrawingBuilderOptions>>(nameof(watermark)))
        {
            Watermark = watermark;
        }


        /// <summary>
        /// 水印图画。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IWatermarkService"/>。
        /// </value>
        public IWatermarkService Watermark { get; }

        /// <summary>
        /// 图像文件扩展名集合。
        /// </summary>
        public string[] ImageExtensions => Options.ImageExtensions.Split(',');

        /// <summary>
        /// 过滤器品质。
        /// </summary>
        public SKFilterQuality FilterQuality { get; set; } = SKFilterQuality.Medium;


        /// <summary>
        /// 删除缩放方案集合图片。
        /// </summary>
        /// <param name="imageDirectory">给定的图像目录。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含删除张数的异步操作。</returns>
        public Task<int> DeleteScalesByDirectoryAsync(string imageDirectory, CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                var count = 0;
                var files = Directory.EnumerateFiles(imageDirectory, "*.*", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);

                    foreach (var scale in Options.Scales)
                    {
                        if (fileName.Contains(scale.Suffix))
                        {
                            File.Delete(file);
                            count++;
                        }
                    }
                }

                return count;
            });
        }


        /// <summary>
        /// 绘制缩放文件集合。
        /// </summary>
        /// <param name="imageDirectory">给定的图像目录。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含处理张数的异步操作。</returns>
        public Task<int> DrawFilesByDirectoryAsync(string imageDirectory, CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                var count = 0;
                var files = Directory.EnumerateFiles(imageDirectory, "*.*", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    if (IsImageFile(file))
                    {
                        DrawFile(file, null);
                        count++;
                    }
                }

                return count;
            });
        }


        /// <summary>
        /// 绘制缩放文件集合。
        /// </summary>
        /// <param name="imagePaths">给定的图像路径集合。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含处理张数的异步操作。</returns>
        public Task<int> DrawFilesAsync(IEnumerable<string> imagePaths, CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                var count = 0;

                foreach (var file in imagePaths)
                {
                    if (IsImageFile(file))
                    {
                        DrawFile(file, null);
                        count++;
                    }
                }

                return count;
            });
        }


        /// <summary>
        /// 绘制缩放文件。
        /// </summary>
        /// <param name="imagePath">给定的图像路径。</param>
        /// <param name="savePathTemplate">给定的保存路径模板（默认同图像路径）。</param>
        /// <returns>返回布尔值。</returns>
        public bool DrawFile(string imagePath, string savePathTemplate = null)
        {
            if (Options.Scales.Count < 1)
            {
                Logger.LogWarning("Scale options is not found.");
                return false;
            }

            using (var srcBmp = SKBitmap.Decode(imagePath))
            {
                var imageSize = new Size(srcBmp.Width, srcBmp.Height);

                // 循环缩放方案
                foreach (var s in Options.Scales)
                {
                    // 如果源图尺寸小于缩放尺寸，则跳过当前方案
                    if (imageSize.Width <= s.MaxSize.Width && imageSize.Height <= s.MaxSize.Height)
                        continue;

                    // 计算等比例缩放尺寸
                    var scaleSize = ScaleSize(imageSize, s.MaxSize);
                    var scaleInfo = new SKImageInfo(scaleSize.Width, scaleSize.Height,
                        srcBmp.Info.ColorType, srcBmp.Info.AlphaType);

                    Logger.LogDebug($"Scale image file: {imagePath}");
                    Logger.LogDebug($"Scale info: width={scaleSize.Width}, height={scaleSize.Height}, colorType={srcBmp.Info.ColorType.AsEnumName()}, alphaType={srcBmp.Info.AlphaType.AsEnumName()}");

                    // 按比例缩放
                    using (var bmp = srcBmp.Resize(scaleInfo, FilterQuality))
                    {
                        // 引入水印
                        if (s.Watermark != WatermarkMode.None && Watermark is InternalWatermarkService internalWatermark)
                        {
                            using (var canvas = new SKCanvas(bmp))
                            {
                                internalWatermark.DrawCore(canvas, scaleSize, s.Watermark);

                                Logger.LogDebug($"Watermark image file: {imagePath}");
                                Logger.LogDebug($"Watermark mode: {s.Watermark.AsEnumName()}");
                            }
                        }
                        
                        var skFormat = Options.ImageFormat.AsOutputEnumByName<ImageFormat, SKEncodedImageFormat>();

                        using (var img = SKImage.FromBitmap(bmp))
                        using (var data = img.Encode(skFormat, Options.Quality))
                        {
                            // 设定文件中间名（如果后缀为空，则采用时间周期）
                            var middleName = s.Suffix.EnsureString(() => DateTime.Now.Ticks.ToString());

                            // 设定缩放保存路径
                            var scaleSavePath = savePathTemplate.EnsureString(imagePath);
                            scaleSavePath = scaleSavePath.ChangeFileName((baseName, extension) =>
                            {
                                // 添加后缀名
                                return baseName + middleName + extension;
                            });

                            using (var fs = new FileStream(scaleSavePath, FileMode.OpenOrCreate))
                            {
                                data.SaveTo(fs);
                                Logger.LogDebug($"Scale image file save as: {scaleSavePath}");
                            }
                        }
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// 按比例缩放尺寸。
        /// </summary>
        /// <param name="rawSize">给定的原始尺寸。</param>
        /// <param name="maxSize">给定的最大尺寸。</param>
        /// <returns>返回计算后的缩放尺寸。</returns>
        private Size ScaleSize(Size rawSize, Size maxSize)
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
        /// 是否为图像文件路径。
        /// </summary>
        /// <param name="path">给定的文件路径。</param>
        /// <returns>返回布尔值。</returns>
        private bool IsImageFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            var lower = path.ToLower();
            foreach (var ext in ImageExtensions)
            {
                if (lower.EndsWith(ext))
                    return true;
            }

            return false;
        }

    }
}
