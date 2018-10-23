#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.IO;

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 文件描述符。
    /// </summary>
    public class FileDescriptor : AbstractFileSystem<FileDescriptor>, IFileSystem, IEquatable<FileDescriptor>
    {
        /// <summary>
        /// 构造一个 <see cref="FileDescriptor"/> 实例。
        /// </summary>
        /// <param name="info">给定的 <see cref="FileInfo"/>。</param>
        public FileDescriptor(FileInfo info)
            : base(info)
        {
            Size = info.Length;
            FormatSize = info.Length.FormatCapacityUnitString();
        }

        
        /// <summary>
        /// 存储目录。
        /// </summary>
        public DirectoryDescriptor Directory { get; set; }
    }
}
