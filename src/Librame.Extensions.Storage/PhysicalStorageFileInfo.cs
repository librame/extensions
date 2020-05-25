#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.FileProviders;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 物理存储文件信息。
    /// </summary>
    public class PhysicalStorageFileInfo : IStorageFileInfo
    {
        /// <summary>
        /// 构造一个 <see cref="PhysicalStorageFileInfo"/>。
        /// </summary>
        /// <param name="info">给定的 <see cref="FileInfo"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public PhysicalStorageFileInfo(FileInfo info)
        {
            info.NotNull(nameof(info));

            Exists = info.Exists;
            Length = info.Length;
            PhysicalPath = info.FullName;
            Name = info.Name;
            LastModified = info.LastWriteTimeUtc;
            IsDirectory = false;
        }

        /// <summary>
        /// 构造一个 <see cref="PhysicalStorageFileInfo"/>。
        /// </summary>
        /// <param name="info">给定的 <see cref="IFileInfo"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public PhysicalStorageFileInfo(IFileInfo info)
        {
            info.NotNull(nameof(info));

            Exists = info.Exists;
            Length = info.Length;
            PhysicalPath = info.PhysicalPath;
            Name = info.Name;
            LastModified = info.LastModified;
            IsDirectory = info.IsDirectory;
        }


        /// <summary>
        /// 是否存在。
        /// </summary>
        public bool Exists { get; }

        /// <summary>
        /// 大小。
        /// </summary>
        public long Length { get; }

        /// <summary>
        /// 物理路径。
        /// </summary>
        public string PhysicalPath { get; }

        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 最后修改时间。
        /// </summary>
        public DateTimeOffset LastModified { get; }

        /// <summary>
        /// 是目录。
        /// </summary>
        public bool IsDirectory { get; }


        /// <summary>
        /// 创建读取流。
        /// </summary>
        /// <returns>返回 <see cref="Stream"/>。</returns>
        public Stream CreateReadStream()
        {
            // 将缓冲区大小设置为 1，以防止 FileStream 分配它的内部缓冲区 0 导致构造函数抛出异常
            var bufferSize = 1;

            return new FileStream(
                PhysicalPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite,
                bufferSize,
                FileOptions.Asynchronous | FileOptions.SequentialScan);
        }

        /// <summary>
        /// 创建写入流。
        /// </summary>
        /// <returns>返回 <see cref="Stream"/>。</returns>
        public Stream CreateWriteStream()
        {
            // 将缓冲区大小设置为 1，以防止 FileStream 分配它的内部缓冲区 0 导致构造函数抛出异常
            var bufferSize = 1;

            return new FileStream(
                PhysicalPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.ReadWrite,
                bufferSize,
                FileOptions.Asynchronous | FileOptions.SequentialScan);
        }
    }
}
