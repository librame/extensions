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

namespace Librame.Extensions.Storage
{
    using Core;

    /// <summary>
    /// 存储构建器选项。
    /// </summary>
    public class StorageBuilderOptions : AbstractBuilderOptions
    {
        /// <summary>
        /// 缓冲区大小（默认使用 512）。
        /// </summary>
        public int BufferSize { get; set; }
            = 512;


        /// <summary>
        /// 文件系统。
        /// </summary>
        public FileSystemOptions FileSystem { get; set; }
            = new FileSystemOptions();
    }


    /// <summary>
    /// 文件系统选项。
    /// </summary>
    public class FileSystemOptions
    {
        /// <summary>
        /// 初始路径。
        /// </summary>
        public string InitPath { get; set; }
            = Directory.GetCurrentDirectory();

        /// <summary>
        /// 查找模式。
        /// </summary>
        public string SearchPattern { get; set; }
            = "*";

        /// <summary>
        /// 查找选项枚举。
        /// </summary>
        public SearchOption SearchOption { get; set; }
            = SearchOption.TopDirectoryOnly;
    }
}
