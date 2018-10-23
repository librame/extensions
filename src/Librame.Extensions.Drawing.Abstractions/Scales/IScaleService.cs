#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Librame.Extensions.Drawing
{
    /// <summary>
    /// 缩放服务接口。
    /// </summary>
    public interface IScaleService : IDrawingService
    {
        /// <summary>
        /// 水印图画。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IWatermarkService"/>。
        /// </value>
        IWatermarkService Watermark { get; }

        /// <summary>
        /// 绘图选项。
        /// </summary>
        /// <value>
        /// 返回 <see cref="DefaultDrawingBuilderOptions"/>。
        /// </value>
        DefaultDrawingBuilderOptions Options { get; }

        /// <summary>
        /// 图像文件扩展名集合。
        /// </summary>
        string[] ImageExtensions { get; }


        /// <summary>
        /// 删除缩放方案集合图片。
        /// </summary>
        /// <param name="imageDirectory">给定的图像目录。</param>
        /// <returns>返回删除张数。</returns>
        int DeleteScalesByDirectory(string imageDirectory);


        /// <summary>
        /// 绘制缩放文件集合。
        /// </summary>
        /// <param name="imageDirectory">给定的图像目录。</param>
        /// <returns>返回一个包含处理张数的异步操作。</returns>
        Task<int> DrawFilesByDirectory(string imageDirectory);


        /// <summary>
        /// 绘制缩放文件集合。
        /// </summary>
        /// <param name="imagePaths">给定的图像路径集合。</param>
        /// <returns>返回一个包含处理张数的异步操作。</returns>
        Task<int> DrawFiles(IEnumerable<string> imagePaths);


        /// <summary>
        /// 绘制缩放文件。
        /// </summary>
        /// <param name="imagePath">给定的图像路径。</param>
        /// <param name="savePathTemplate">给定的保存路径模板（可选；默认同图像路径）。</param>
        /// <returns>返回一个包含布尔值的异步操作。</returns>
        Task<bool> DrawFile(string imagePath, string savePathTemplate = null);
    }
}
