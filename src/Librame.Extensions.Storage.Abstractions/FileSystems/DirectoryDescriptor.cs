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
using System.Collections.Generic;
using System.IO;

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 目录描述符。
    /// </summary>
    public class DirectoryDescriptor : AbstractFileSystem<DirectoryDescriptor>, IFileSystem, IEquatable<DirectoryDescriptor>
    {
        /// <summary>
        /// 构造一个 <see cref="DirectoryDescriptor"/> 实例。
        /// </summary>
        /// <param name="info">给定的 <see cref="DirectoryInfo"/>。</param>
        /// <param name="filter">给定的文件过滤规则。</param>
        public DirectoryDescriptor(DirectoryInfo info, string filter = null)
            : base(info)
        {
            Subdirs = new List<DirectoryDescriptor>();
            Subfiles = new List<FileDescriptor>();
            Filter = filter ?? "*.*";

            Initialize();
        }


        private void Initialize()
        {
            var children = (Info as DirectoryInfo).EnumerateFileSystemInfos(Filter, SearchOption.TopDirectoryOnly);
            if (children.IsEmpty()) return;

            foreach (var child in children)
            {
                if (Directory.Exists(child.FullName))
                {
                    var info = new DirectoryInfo(child.FullName);
                    var dir = new DirectoryDescriptor(info, Filter);
                    dir.Parent = this;

                    Size += dir.Size;
                    Subdirs.Add(dir);
                }
                else if (File.Exists(child.FullName))
                {
                    var info = new FileInfo(child.FullName);
                    Size += info.Length;

                    var file = new FileDescriptor(info);
                    file.Directory = this;

                    Subfiles.Add(file);
                }
                else
                {
                    //
                }
            }
            
            FormatSize = Size.FormatCapacityUnitString();
        }


        /// <summary>
        /// 子目录列表。
        /// </summary>
        public IList<DirectoryDescriptor> Subdirs { get; }

        /// <summary>
        /// 文件列表。
        /// </summary>
        public IList<FileDescriptor> Subfiles { get; }

        /// <summary>
        /// 文件过滤规则。
        /// </summary>
        public string Filter { get; }

        
        /// <summary>
        /// 父目录（默认为 NULL）。
        /// </summary>
        public DirectoryDescriptor Parent { get; set; }


        /// <summary>
        /// 链式查找指定目录。
        /// </summary>
        /// <param name="dirPath">给定的目录路径。</param>
        /// <returns>返回 <see cref="DirectoryDescriptor"/>。</returns>
        public DirectoryDescriptor LookupDirectory(string dirPath)
        {
            // 如果是当前目录，则直接返回
            if (Path == dirPath) return this;

            DirectoryDescriptor dir = null;

            if (Subdirs.IsEmpty()) return dir;

            foreach (var child in Subdirs)
            {
                dir = child.LookupDirectory(dirPath);

                if (dir.IsNotDefault())
                    return dir;
            }
            
            return dir;
        }


        /// <summary>
        /// 链式查找指定文件。
        /// </summary>
        /// <param name="filePath">给定的文件路径。</param>
        /// <returns>返回 <see cref="FileDescriptor"/>。</returns>
        public FileDescriptor LookupFile(string filePath)
        {
            FileDescriptor file = null;

            // 查找兄弟文件
            if (Subfiles.IsEmpty()) return file;

            foreach (var child in Subfiles)
            {
                if (child.Path == filePath)
                {
                    file = child;
                    break;
                }
            }

            // 查找子目录
            if (file.IsDefault() && Subdirs.IsNotEmpty())
            {
                foreach (var child in Subdirs)
                {
                    file = child.LookupFile(filePath);

                    if (file.IsNotDefault())
                        return file;
                }
            }

            return file;
        }

    }
}
