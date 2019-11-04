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
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Storage
{
    using Core;

    /// <summary>
    /// 存储构建器选项。
    /// </summary>
    public class StorageBuilderOptions : IExtensionBuilderOptions
    {
        /// <summary>
        /// 缓冲区大小（默认使用 512）。
        /// </summary>
        public int BufferSize { get; set; }
            = 512;


        /// <summary>
        /// 文件提供程序列表。
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public IList<IStorageFileProvider> FileProviders { get; set; }
            = new List<IStorageFileProvider>();


        /// <summary>
        /// 文件传输。
        /// </summary>
        public FileTransferOptions FileTransfer { get; set; }
            = new FileTransferOptions();
    }


    /// <summary>
    /// 文件传输选项。
    /// </summary>
    public class FileTransferOptions
    {
        /// <summary>
        /// 超时（毫秒）。
        /// </summary>
        public int Timeout { get; set; }
            = 10000;

        /// <summary>
        /// 浏览器代理。
        /// </summary>
        public string UserAgent { get; set; }
            = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36";
    }
}
