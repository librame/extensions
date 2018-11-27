#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.IO;
using System.Threading.Tasks;

namespace Librame.Extensions.Drawing
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
        /// <returns>返回一个包含是否成功的异步操作。</returns>
        Task<bool> DrawFile(string imagePath, string savePath, WatermarkMode mode = WatermarkMode.Text);


        /// <summary>
        /// 绘制水印流。
        /// </summary>
        /// <param name="imagePath">给定的验证码。</param>
        /// <param name="target">给定的目标流。</param>
        /// <param name="mode">给定的水印模绘制式（可选；默认使用文本模式）。</param>
        /// <returns>返回一个包含是否成功的异步操作。</returns>
        Task<bool> DrawStream(string imagePath, Stream target, WatermarkMode mode = WatermarkMode.Text);


        /// <summary>
        /// 绘制水印字节数组。
        /// </summary>
        /// <param name="imagePath">给定的图像路径。</param>
        /// <param name="mode">给定的水印模绘制式（可选；默认使用文本模式）。</param>
        /// <returns>返回一个包含图像字节数组的异步操作。</returns>
        Task<byte[]> DrawBytes(string imagePath, WatermarkMode mode = WatermarkMode.Text);
    }
}
