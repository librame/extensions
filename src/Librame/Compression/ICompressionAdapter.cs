#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using ICSharpCode.SharpZipLib.Zip;
using System;

namespace Librame.Compression
{
    /// <summary>
    /// 压缩插件接口。
    /// </summary>
    public interface ICompressionAdapter : Adaptation.IAdapter
    {
        /// <summary>
        /// 创建压缩文件。
        /// </summary>
        /// <param name="zipFileName">给定的压缩文件名。</param>
        /// <param name="addAction">添加要压缩的文件或文件夹对象动作。</param>
        void Compress(string zipFileName, Action<ZipFile> addAction);


        /// <summary>
        /// 更新压缩文件。
        /// </summary>
        /// <param name="zipFileName">给定的压缩文件名。</param>
        /// <param name="upgradeAction">要更新的动作方法。</param>
        void UpgradeCompress(string zipFileName, Action<ZipFile> upgradeAction);


        /// <summary>
        /// 遍历压缩文件。
        /// </summary>
        /// <param name="zipFileName">给定的压缩文件名。</param>
        /// <param name="foreachAction">给定的解压目录。</param>
        void ForeachCompress(string zipFileName, Action<ZipEntry> foreachAction);


        /// <summary>
        /// 解压文件。
        /// </summary>
        /// <param name="zipFileName">给定的压缩文件名。</param>
        /// <param name="destinationDirectory">给定的解压目录。</param>
        /// <param name="fileFilter">给定的文件过滤器。</param>
        void Decompress(string zipFileName, string destinationDirectory, string fileFilter = null);

    }
}
