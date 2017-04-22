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
    /// 默认压缩适配器。
    /// </summary>
    public class DefaultCompressionAdapter : AbstractCompressionAdapter, ICompressionAdapter
    {
        
        /// <summary>
        /// 创建压缩文件。
        /// </summary>
        /// <param name="zipFileName">给定的压缩文件名。</param>
        /// <param name="addAction">添加要压缩的文件或文件夹对象动作。</param>
        public virtual void Compress(string zipFileName, Action<ZipFile> addAction)
        {
            using (var zip = ZipFile.Create(zipFileName))
            {
                zip.BeginUpdate();

                //zip.SetComment();
                //zip.Add();
                //zip.AddDirectory();
                addAction?.Invoke(zip);

                zip.CommitUpdate();
            }
        }


        /// <summary>
        /// 更新压缩文件。
        /// </summary>
        /// <param name="zipFileName">给定的压缩文件名。</param>
        /// <param name="upgradeAction">要更新的动作方法。</param>
        public virtual void UpgradeCompress(string zipFileName, Action<ZipFile> upgradeAction)
        {
            using (var zip = new ZipFile(zipFileName))
            {
                zip.BeginUpdate();

                upgradeAction?.Invoke(zip);

                zip.CommitUpdate();
            }
        }


        /// <summary>
        /// 遍历压缩文件。
        /// </summary>
        /// <param name="zipFileName">给定的压缩文件名。</param>
        /// <param name="foreachAction">给定的解压目录。</param>
        public virtual void ForeachCompress(string zipFileName, Action<ZipEntry> foreachAction)
        {
            using (var zip = new ZipFile(zipFileName))
            {
                foreach (ZipEntry z in zip)
                {
                    foreachAction?.Invoke(z);
                }
            }
        }


        /// <summary>
        /// 解压文件。
        /// </summary>
        /// <param name="zipFileName">给定的压缩文件名。</param>
        /// <param name="destinationDirectory">给定的解压目录。</param>
        /// <param name="fileFilter">给定的文件过滤器。</param>
        public virtual void Decompress(string zipFileName, string destinationDirectory, string fileFilter = null)
        {
            if (string.IsNullOrEmpty(fileFilter))
                fileFilter = "";

            var fz = new FastZip();
            fz.ExtractZip(zipFileName, destinationDirectory, fileFilter);
        }

    }
}
