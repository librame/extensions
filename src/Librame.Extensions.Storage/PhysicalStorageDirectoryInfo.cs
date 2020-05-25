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
    using Resources;

    /// <summary>
    /// 物理存储目录信息。
    /// </summary>
    public class PhysicalStorageDirectoryInfo : IStorageFileInfo
    {
        /// <summary>
        /// 构造一个 <see cref="PhysicalStorageDirectoryInfo"/>。
        /// </summary>
        /// <param name="info">给定的 <see cref="DirectoryInfo"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public PhysicalStorageDirectoryInfo(DirectoryInfo info)
        {
            info.NotNull(nameof(info));

            Exists = info.Exists;
            Length = -1;
            PhysicalPath = info.FullName;
            Name = info.Name;
            LastModified = info.LastWriteTimeUtc;
            IsDirectory = true;
        }

        /// <summary>
        /// 构造一个 <see cref="PhysicalStorageDirectoryInfo"/>。
        /// </summary>
        /// <param name="info">给定的 <see cref="IFileInfo"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public PhysicalStorageDirectoryInfo(IFileInfo info)
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
        /// 总是引发异常，因为目录上不支持读取流。
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// 总是抛出异常。
        /// </exception>
        /// <returns>没有返回。</returns>
        public Stream CreateReadStream()
            => throw new InvalidOperationException(InternalResource.InvalidOperationExceptionCreateStream);

        /// <summary>
        /// 总是引发异常，因为目录上不支持写入流。
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// 总是抛出异常。
        /// </exception>
        /// <returns>没有返回。</returns>
        public Stream CreateWriteStream()
            => throw new InvalidOperationException(InternalResource.InvalidOperationExceptionCreateStream);
    }
}
