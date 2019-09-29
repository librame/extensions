#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Internal;
using Microsoft.Extensions.FileProviders.Physical;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 物理存储目录内容集合。
    /// </summary>
    public class PhysicalStorageDirectoryContents : IStorageDirectoryContents
    {
        private IEnumerable<IStorageFileInfo> _entries;
        private readonly string _directory;
        private readonly ExclusionFilters _filters;


        /// <summary>
        /// 构造一个 <see cref="PhysicalStorageDirectoryContents"/>。
        /// </summary>
        /// <param name="directory">给定的目录。</param>
        public PhysicalStorageDirectoryContents(string directory)
            : this(directory, ExclusionFilters.Sensitive)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="PhysicalStorageDirectoryContents"/>。
        /// </summary>
        /// <param name="directory">给定的目录。</param>
        /// <param name="filters">指定排除哪些文件或目录。</param>
        public PhysicalStorageDirectoryContents(string directory, ExclusionFilters filters)
        {
            _directory = directory.NotEmpty(nameof(directory));
            _filters = filters;

            Exists = Directory.Exists(_directory);
        }

        /// <summary>
        /// 构造一个 <see cref="PhysicalStorageDirectoryContents"/>。
        /// </summary>
        /// <param name="contents">给定的 <see cref="IDirectoryContents"/>。</param>
        public PhysicalStorageDirectoryContents(IDirectoryContents contents)
        {
            contents.NotEmpty(nameof(contents));

            _entries = contents.Select<IFileInfo, IStorageFileInfo>(info =>
            {
                if (info is PhysicalFileInfo file)
                {
                    return new PhysicalStorageFileInfo(file);
                }
                else if (info is PhysicalDirectoryInfo dir)
                {
                    return new PhysicalStorageDirectoryInfo(dir);
                }
                // shouldn't happen unless BCL introduces new implementation of base type
                throw new InvalidOperationException("Unexpected type of FileSystemInfo");
            });

            Exists = (contents is PhysicalDirectoryContents physical) && physical.Exists;
        }


        /// <summary>
        /// 是否存在。
        /// </summary>
        public bool Exists { get; }


        /// <summary>
        /// 获取枚举器。
        /// </summary>
        /// <returns>返回 <see cref="IEnumerable{IStorageFileInfo}"/>。</returns>
        public IEnumerator<IStorageFileInfo> GetEnumerator()
        {
            if (_directory.IsNotEmpty())
                EnsureInitialized(); // 目录模式

            return _entries.GetEnumerator();
        }

        /// <summary>
        /// 获取枚举器。
        /// </summary>
        /// <returns>返回 <see cref="IFileInfo"/>。</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void EnsureInitialized()
        {
            try
            {
                _entries = new DirectoryInfo(_directory)
                    .EnumerateFileSystemInfos()
                    .Where(info => !info.IsExcluded(_filters))
                    .Select<FileSystemInfo, IStorageFileInfo>(info =>
                    {
                        if (info is FileInfo file)
                        {
                            return new PhysicalStorageFileInfo(file);
                        }
                        else if (info is DirectoryInfo dir)
                        {
                            return new PhysicalStorageDirectoryInfo(dir);
                        }
                        // shouldn't happen unless BCL introduces new implementation of base type
                        throw new InvalidOperationException("Unexpected type of FileSystemInfo");
                    });
            }
            catch (Exception ex) when (ex is DirectoryNotFoundException || ex is IOException)
            {
                _entries = Enumerable.Empty<IStorageFileInfo>();
            }
        }

    }
}
