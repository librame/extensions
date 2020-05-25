#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Drawing.Services
{
    /// <summary>
    /// 水印服务接口。
    /// </summary>
    public interface IWatermarkService : IDrawingService
    {
        /// <summary>
        /// 绘制水印文件。
        /// </summary>
        /// <param name="imagePath">给定的图像路径。</param>
        /// <param name="savePath">给定的保存路径。</param>
        /// <param name="mode">给定的水印模绘制式（可选；默认使用文本模式）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否成功的异步操作。</returns>
        Task<bool> DrawFileAsync(string imagePath, string savePath,
            WatermarkMode mode = WatermarkMode.Text,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 绘制水印流。
        /// </summary>
        /// <param name="imagePath">给定的图像路径。</param>
        /// <param name="target">给定的目标流。</param>
        /// <param name="mode">给定的水印模绘制式（可选；默认使用文本模式）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否成功的异步操作。</returns>
        Task<bool> DrawStreamAsync(string imagePath, Stream target,
            WatermarkMode mode = WatermarkMode.Text,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 绘制水印字节数组。
        /// </summary>
        /// <param name="imagePath">给定的图像路径。</param>
        /// <param name="mode">给定的水印模绘制式（可选；默认使用文本模式）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含图像字节数组的异步操作。</returns>
        Task<byte[]> DrawBytesAsync(string imagePath, WatermarkMode mode = WatermarkMode.Text,
            CancellationToken cancellationToken = default);
    }
}
