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

namespace Librame.Extensions.Storage.Builders
{
    using Core.Builders;
    using Services;

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
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public IList<IStorageFileProvider> FileProviders { get; }
            = new List<IStorageFileProvider>();


        /// <summary>
        /// 文件传输。
        /// </summary>
        public FileTransferOptions FileTransfer { get; set; }
            = new FileTransferOptions();
    }
}
