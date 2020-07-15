#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Drawing.Services
{
    using Core.Builders;
    using Core.Services;
    using Drawing.Builders;
    using Drawing.Options;
    using Drawing.Resources;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class ScaleService : AbstractExtensionBuilderService<DrawingBuilderOptions>, IScaleService
    {
        public ScaleService(IClockService clock, IWatermarkService watermark)
            : base(watermark.CastTo<IWatermarkService, AbstractExtensionBuilderService<DrawingBuilderOptions>>(nameof(watermark)))
        {
            Clock = clock.NotNull(nameof(clock));
            Watermark = watermark;
        }


        public IClockService Clock { get; }

        public IWatermarkService Watermark { get; }

        public IExtensionBuilderDependency Dependency
            => Watermark.Dependency;

        public IReadOnlyList<string> ImageExtensions
            => Options.ImageExtensions.Split(',');

        public SKEncodedImageFormat CurrentImageFormat
            => Options.ImageFormat.MatchEnum<ImageFormat, SKEncodedImageFormat>();

        public SKFilterQuality FilterQuality { get; set; }
            = SKFilterQuality.Medium;


        public Task<int> DeleteScalesByDirectoryAsync(string imageDirectory, IEnumerable<ScaleOptions> scales = null,
            CancellationToken cancellationToken = default)
        {
            if (scales.IsNull())
                scales = Options.Scales ?? throw new ArgumentNullException(InternalResource.ScaleOptionsIsEmpty);

            return cancellationToken.RunOrCancelAsync(() =>
            {
                var count = 0;
                var files = Directory.EnumerateFiles(imageDirectory, "*.*", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);

                    foreach (var s in scales)
                    {
                        if (fileName.CompatibleContains(s.Suffix))
                        {
                            File.Delete(file);
                            count++;
                        }
                    }
                }

                return count;
            });
        }


        public Task<int> DrawFilesByDirectoryAsync(string imageDirectory, IEnumerable<ScaleOptions> scales = null,
            CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunOrCancelAsync(() =>
            {
                var count = 0;
                var files = Directory.EnumerateFiles(imageDirectory, "*.*", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    if (IsImageFile(file))
                    {
                        DrawFile(file, scales, savePathTemplate: null);
                        count++;
                    }
                }

                return count;
            });
        }

        public Task<int> DrawFilesAsync(IEnumerable<string> imagePaths, IEnumerable<ScaleOptions> scales = null,
            CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunOrCancelAsync(() =>
            {
                var count = 0;

                foreach (var file in imagePaths)
                {
                    if (IsImageFile(file))
                    {
                        DrawFile(file, scales, savePathTemplate: null);
                        count++;
                    }
                }

                return count;
            });
        }

        public bool DrawFile(string imagePath, IEnumerable<ScaleOptions> scales = null, string savePathTemplate = null)
        {
            if (scales.IsNull())
                scales = Options.Scales ?? throw new ArgumentNullException(InternalResource.ScaleOptionsIsEmpty);

            using (var srcBmp = SKBitmap.Decode(imagePath))
            {
                var imageSize = new Size(srcBmp.Width, srcBmp.Height);

                // 循环缩放方案
                foreach (var s in scales)
                {
                    // 如果源图尺寸小于缩放尺寸，则跳过当前方案
                    if (imageSize.Width <= s.MaxSize.Width && imageSize.Height <= s.MaxSize.Height)
                        continue;

                    // 计算等比例缩放尺寸
                    var scaleSize = ImageHelper.ScaleSize(imageSize, s.MaxSize);
                    var scaleInfo = new SKImageInfo(scaleSize.Width, scaleSize.Height,
                        srcBmp.Info.ColorType, srcBmp.Info.AlphaType);

                    Logger.LogDebug($"Scale image file: {imagePath}");
                    Logger.LogDebug($"Scale info: width={scaleSize.Width}, height={scaleSize.Height}, colorType={srcBmp.Info.ColorType.AsEnumName()}, alphaType={srcBmp.Info.AlphaType.AsEnumName()}");

                    // 按比例缩放
                    using (var bmp = srcBmp.Resize(scaleInfo, FilterQuality))
                    {
                        // 引入水印
                        if (s.Watermark != WatermarkMode.None && Watermark is WatermarkService internalWatermark)
                        {
                            using (var canvas = new SKCanvas(bmp))
                            {
                                internalWatermark.DrawCore(canvas, scaleSize, s.Watermark);

                                Logger.LogDebug($"Watermark image file: {imagePath}");
                                Logger.LogDebug($"Watermark mode: {s.Watermark.AsEnumName()}");
                            }
                        }
                        
                        using (var img = SKImage.FromBitmap(bmp))
                        using (var data = img.Encode(CurrentImageFormat, Options.Quality))
                        {
                            if (data.IsNull())
                                throw new InvalidOperationException(InternalResource.InvalidOperationExceptionUnsupportedImageFormat);

                            // 设定文件中间名（如果后缀为空，则采用时间周期）
                            var middleName = s.Suffix.NotEmptyOrDefault(() =>
                            {
                                return Clock.GetNowOffsetAsync().ConfigureAwaitCompleted().Ticks.ToString(CultureInfo.InvariantCulture);
                            });

                            // 设定缩放保存路径
                            var scaleSavePath = savePathTemplate.NotEmptyOrDefault(imagePath);
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


        private bool IsImageFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            foreach (var ext in ImageExtensions)
            {
                if (path.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

    }
}
